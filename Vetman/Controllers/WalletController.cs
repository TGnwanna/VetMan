using AutoMapper;
using Core.Config;
using Core.Db;
using Core.Models;
using Core.ViewModels;
using Logic.IHelpers;
using Logic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static Logic.AppHttpContext;

namespace Vetman.Controllers
{
    [SessionTimeout]
    public class WalletController : Controller
    {
        private readonly IWalletHelper _walletHelper;
        private readonly IMapper _mapper;
        private readonly IUserHelper _userHelper;
        private readonly AppDbContext _context;
        private readonly IPaystackHelper _paystackHelper;
        private readonly IGeneralConfiguration _generalConfiguration;
        private readonly IEmailService _emailService;
        private readonly ISubscriptionHelper _subscriptionHelper;

        public WalletController(IWalletHelper walletHelper, IMapper mapper, IUserHelper userHelper, AppDbContext context, IPaystackHelper paystackHelper,
            IGeneralConfiguration generalConfiguration, IEmailService emailService, ISubscriptionHelper subscriptionHelper)
        {
            _walletHelper = walletHelper;
            _mapper = mapper;
            _userHelper = userHelper;
            _context = context;
            _paystackHelper = paystackHelper;
            _generalConfiguration = generalConfiguration;
            _emailService = emailService;
            _subscriptionHelper = subscriptionHelper;
        }

        public IActionResult Index()
        {
            ViewBag.Layout = _userHelper.GetRoleLayout();
            var transactionHistory = _walletHelper.GetCompanyTransactions();
            if (transactionHistory.Count() > 0)
            {
                ViewBag.WalletId = transactionHistory?.FirstOrDefault()?.WalletId;
                ViewBag.Balance = transactionHistory?.FirstOrDefault()?.Balance;
            }
            else
            {
                ViewBag.WalletId = Guid.Empty;
                ViewBag.Balance = 0.00;
            }

            return View(transactionHistory);
        }

        public IActionResult FundWallet(Guid id)
        {
            var userData = Utitily.GetCurrentUser();
            if (userData.Id == null)
            {
                userData = _userHelper.UpdateSessionAsync(userData.UserName).Result;

            }
            var company = _context.CompanyBranches.Where(x => x.Id == userData.CompanyBranchId && x.CompanyId != Guid.Empty).Include(x => x.Company).FirstOrDefault();
            var companyWallet = new WalletHistoryViewModel()
            {
                WalletId = id,
                UserId = userData.Id,
                CompanyBranchId = userData.CompanyBranchId,
                CompanyId = company?.CompanyId,
            };
            return View(companyWallet);
        }

        public async Task<IActionResult> MakePayment(string wallet)
        {
            if (wallet != null)
            {
                var paymentDetails = JsonConvert.DeserializeObject<WalletHistoryViewModel>(wallet);
                var response = await _walletHelper.GeneratePaymentParameters(paymentDetails);
                if (response != null)
                    return Json(new { isError = false, isPayStack = true, paystackUrl = response.data.authorization_url });
            }
            return Json(new { isError = true, msg = "Order payment couldn't be reached,Please retry later." });
        }

        public IActionResult PaystackResponseFeedback(Paystack paystack)
        {
            var subscription = _context.PaystackSubscriptions.Where(x => x.reference == paystack.reference).FirstOrDefault();
            if (subscription != null)
            {
                _subscriptionHelper.GetPaymentResponse(subscription);
                return RedirectToAction("Index", "Subscription");
            }
            else
            {
                var isGuestBooking = _context.Paystacks.Where(x => x.reference == paystack.reference).FirstOrDefault();
                if (isGuestBooking != null && isGuestBooking.GuestBookingId != Guid.Empty)
                {
                    var guestBookingPaymentResponse = _paystackHelper.VerifyPayment(paystack).Result;
                    if (guestBookingPaymentResponse != null)
                    {
                        string toEmail = _generalConfiguration.AdminEmail;
                        string subject = "PAYMENT NOTIFICATION";
                        string message = "&#8358;" + paystack.amount + " has been credited to your Account by " + guestBookingPaymentResponse.data.customer.first_name
                            + "Ref No: " + paystack.reference;

                        _emailService.SendEmail(toEmail, subject, message);

                        string customersEmail = guestBookingPaymentResponse.data.customer.email;
                        string header = "PAYMENT NOTIFICATION";
                        string messageCustomers = "<b> REF NO: " + " " + paystack.reference
                            + "<br/> " + " We recieved a desposit of" + " " + "&#8358;" +
                            paystack.amount + ", " + "In to " +
                            "<br/>  " + " <br/>Make sure you  present your reference number for clarification.";
                        _emailService.SendEmail(customersEmail, header, messageCustomers);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    var paymentResponse = _walletHelper.GetPaymentResponse(paystack);
                    return RedirectToAction("Index");
                }
            }           
            return RedirectToAction("Index");
        }
        public IActionResult CheckBalance()
        {
            var balance = _walletHelper.GetBranchBalance();
            return Json(new { isError = false, data = balance });
        }

    }
}
