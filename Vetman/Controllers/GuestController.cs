using Core.Config;
using Core.Db;
using Core.Models;
using Core.ViewModels;
using Logic.Helpers;
using Logic.IHelpers;
using Logic.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Logic.AppHttpContext;

namespace Vetman.Controllers
{
	[SessionTimeout]
	public class GuestController : Controller
	{
		private readonly IAdminHelper _adminHelper;
		private readonly IUserHelper _userHelper;
		private readonly IBookingHelper _bookingHelper;
		private readonly IWalletHelper _walletHelper;
		private readonly AppDbContext _context;
		private readonly IDropdownHelper _dropdownHelper;
		private readonly IPaystackHelper _paystackHelper;
        private readonly IEmailService _emailService;
        private readonly ISubscriptionHelper _subscriptionHelper;
		private readonly IGeneralConfiguration _generalConfiguration;

		public GuestController(AppDbContext context, IUserHelper userHelper, IAdminHelper adminHelper, IBookingHelper bookingHelper, IWalletHelper walletHelper,
            IDropdownHelper dropdownHelper, IPaystackHelper paystackHelper, ISubscriptionHelper subscriptionHelper, IGeneralConfiguration generalConfiguration,
            IEmailService emailService)
		{
			_context = context;
			_userHelper = userHelper;
			_adminHelper = adminHelper;
			_bookingHelper = bookingHelper;
			_walletHelper = walletHelper;
			_dropdownHelper = dropdownHelper;
			_paystackHelper = paystackHelper;
			_subscriptionHelper = subscriptionHelper;
			_generalConfiguration = generalConfiguration;
			_emailService = emailService;

        }

		[HttpGet]
		public IActionResult GuestBooking(Guid id)
		{
			ViewBag.CompanyBranchId = id;
			ViewBag.BookingGroup = _bookingHelper.GetGuestBookingGroups(id);
			return View();
		}
		[HttpPost]
		public IActionResult GuestBooking(string guestBookingViewModel)
		{
			if (guestBookingViewModel != string.Empty)
			{
				var bookingDetails = JsonConvert.DeserializeObject<GuestBookingViewModel>(guestBookingViewModel);
				if (bookingDetails != null)
				{
					if (bookingDetails.BookingId == Guid.Empty)
						return Json(new { isError = true, msg = "Booking Group is required" });

					if (bookingDetails.Quantity == 0)
						return Json(new { isError = true, msg = "Please enter quantity" });

					if (bookingDetails.PhoneNumber == "")
						return Json(new { isError = true, msg = "Please enter phone Number" });
					var guestBooking = _bookingHelper.AddBookingForGuest(bookingDetails);
					if (guestBooking != null)
					{
						var responses = _paystackHelper.GeneratePaymentParameterForGuest(guestBooking);
						if (responses != null)
						{
							return Json(responses);
						}
					}
					return Json(new { isError = true, msg = "Error Occurred" });
				}
			}
			return Json(new { isError = true, msg = "Please fill the form correctly" });
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
                if (isGuestBooking != null && isGuestBooking.GuestBookingId != null)
                {
                    var guestBookingPaymentResponse = _paystackHelper.VerifyPayment(paystack).Result;
                    if (guestBookingPaymentResponse != null)
                    {
                        string toEmail = _generalConfiguration.AdminEmail;
                        string subject = "PAYMENT NOTIFICATION";
                        string message = "&#8358;" + paystack.amount + " has been credited to your Account by " + guestBookingPaymentResponse.data.customer.first_name
                            + " Ref No: " + paystack.reference;

                        _emailService.CallHangfire(toEmail, subject, message);

                        string customersEmail = guestBookingPaymentResponse.data.customer.email;
                        string header = "PAYMENT NOTIFICATION";
                        string messageCustomers = "<b> REF NO: " + " " + paystack.reference
                            + "<br/> " + " We recieved a desposit of" + " " + "&#8358;" +
                            paystack.amount + ", " + "In to " +
                            "<br/>  " + " <br/>Make sure you  present your reference number for clarification.";
                        _emailService.CallHangfire(customersEmail, header, messageCustomers);
                        return RedirectToAction("Index", "Wallet");
                    }
                }
				else
				{
                    var paymentResponse = _walletHelper.GetPaymentResponse(paystack);
                    return RedirectToAction("Index", "Wallet");
                }
            }
            return RedirectToAction("Index", "Wallet");
        }
    }
}
