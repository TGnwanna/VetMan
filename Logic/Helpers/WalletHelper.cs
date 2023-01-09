using AutoMapper;
using Core.Config;
using Core.Db;
using Core.Models;
using Core.ViewModels;
using Logic.IHelpers;
using Logic.Services;
using Microsoft.EntityFrameworkCore;
using static Core.Enums.VetManEnums;
using static Core.Models.PaystackResponse;

namespace Logic.Helpers
{
    public class WalletHelper : IWalletHelper
    {
        
        private readonly AppDbContext _context;
        private readonly IPaystackHelper _paystackHelper;
        private readonly IGeneralConfiguration _generalConfiguration;
        private readonly IEmailService _emailService;
        private readonly IUserHelper _userHelper;

        public WalletHelper( AppDbContext context, IPaystackHelper paystackHelper, IGeneralConfiguration generalConfiguration,
            IEmailService emailService)
        {
           
            _context = context;
            _paystackHelper = paystackHelper;
            _generalConfiguration = generalConfiguration;
            _emailService = emailService;
        }

        public IEnumerable<WalletHistoryViewModel> GetCompanyTransactions()
        {
            var transaction = new List<WalletHistoryViewModel>();
            var loggedInUser = Utitily.GetCurrentUser();
            if (loggedInUser.Id == null)
            {
               // loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName)?.Result;
            }
            if (loggedInUser != null)
            {
                var company = _context.CompanyBranches.Where(x => x.Id == loggedInUser.CompanyBranchId && x.CompanyId != Guid.Empty).Include(x => x.Company).FirstOrDefault();
                if (company != null)
                {
                    var transactionHistory = _context.WalletHistories.Where(x => x.CompanyBranchId == loggedInUser.CompanyBranchId && x.PayStackResponses == PayStackResponses.Completed)
                    .Include(f => f.Wallet).Select(s => new WalletHistoryViewModel()
                    {
                        Id = s.Id,
                        UserId = loggedInUser.Id,
                        Balance = s.Wallet.Balance,
                        Date = s.Date,
                        Amount = s.Amount,
                        WalletId = s.WalletId,
                        CompanyId = company.CompanyId,
                        CompanyBranchId = s.CompanyBranchId,
                        Description = s.Description,
                        TransactionType = s.TransactionType
                    }).OrderByDescending(s => s.Date).ToList();
                    return transactionHistory;
                }
            }
            return transaction;
        }

        public async Task<PaystackRepsonse> GeneratePaymentParameters(WalletHistoryViewModel wallet)
        {
            try
            {
                var paystackResponse = _paystackHelper.MakeOrderPayment(wallet);
                if (paystackResponse != null)
                {
                    var userWallet = _context.WalletHistories.Find(wallet.Id);
                    Paystack paystack = new Paystack()
                    {
                        WalletHistory = userWallet,
                        authorization_url = paystackResponse.data.authorization_url,
                        access_code = paystackResponse.data.access_code,
                        WalletHistoryId = wallet.Id,
                        amount = Convert.ToDecimal(wallet.Amount),
                        transaction_date = DateTime.Now,
                        reference = paystackResponse.data.reference,
                    };
                    _context.Paystacks.Add(paystack);
                    await _context.SaveChangesAsync();
                    return paystackResponse;
                }
                else
                {
                    return null;
                }


            }
            catch (Exception ex)
            {
                var toEmail = _generalConfiguration.AdminEmail;
                var subject = "Generate Payment Parameters Method Exception on SmartEnter";
                _emailService.SendEmail(toEmail, subject, ex.Message);
                throw;
            }
        }

        public bool GetPaymentResponse(Paystack paystack)
        {
            var paymentStatus = _context.Paystacks.Where(x => x.reference == paystack.reference).Include(c => c.WalletHistory).FirstOrDefault();
            if (paymentStatus != null || paymentStatus.WalletHistoryId != Guid.Empty)
            {
                var user = Utitily.GetCurrentUser();
                if(user != null)
                {
                    var completedpayment = _paystackHelper.VerifyPayment(paymentStatus);

                    string toEmail = _generalConfiguration.AdminEmail;
                    string subject = "PAYMENT NOTIFICATION";
                    string message = "&#8358;" + paymentStatus.amount + " has been credited to your Account by " + user.FirstName
                        + "Ref No: " + paystack.reference;

                    _emailService.SendEmail(toEmail, subject, message);

                    string customersEmail = user.Email;
                    string header = "PAYMENT NOTIFICATION";
                    string messageCustomers = "<b> REF NO: " + " " + paystack.reference
                        + "<br/> " + " We recieved a desposit of " + " " + "&#8358; " +
                        paymentStatus.amount + ", " + " In to " +
                        " <br/>  " + " <br/>Make sure you  present your reference number for clarification.";
                    _emailService.SendEmail(customersEmail, header, messageCustomers);

                    return true;
                }
               
            }
          
             return false;
        }

        public bool UpdateWalletHistory(BookingGroupViewModel bookingGroup, double amount, Wallet wallet)
        {
            if (bookingGroup != null && amount != 0 && wallet != null)
            {
                var walletHistory = new WalletHistory()
                {
                    Active = true,
                    Date = DateTime.Now,
                    Description = "Sms",
                    PayStackResponses = PayStackResponses.Completed,
                    Amount = amount,
                    TransactionType = TransactionType.Debit,
                    WalletId = wallet.Id,
                    CompanyBranchId = wallet.CompanyBranchId,
                    Name = wallet.Name,
                };
                _context.WalletHistories.Add(walletHistory);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public double GetBranchBalance()
        {
            var loggedInUser = Utitily.GetCurrentUser();
            
            var wallet = _context.Wallets.Where(x => x.CompanyBranchId == loggedInUser.CompanyBranchId).FirstOrDefault();
            if (wallet != null)
                return wallet.Balance;
            return 0;
        }
    }
}
