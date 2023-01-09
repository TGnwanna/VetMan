using Core.Config;
using Core.Db;
using Core.Models;
using Core.ViewModels;
using Hangfire;
using Logic.IHelpers;
using Logic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Logic.Helpers
{
    public class EmailHelper : IEmailHelper
    {
        private readonly IEmailService _emailService;
        private readonly AppDbContext _context;
        private readonly IGeneralConfiguration _generalConfiguration;
        private readonly IWalletHelper _walletHelper;


        public EmailHelper(AppDbContext context, IEmailService emailService, IGeneralConfiguration generalConfiguration, IWalletHelper walletHelper)
        {
            _context = context;
            _emailService = emailService;
            _generalConfiguration = generalConfiguration;
            _walletHelper = walletHelper;

        }
        /// <summary>
        /// Gets all customer's phone number in a group and send messages across to them when their is change in price
        /// </summary>
        /// <param name="bookingGroupId"></param>
        /// <returns>
        /// bool
        /// </returns>
        public void GetCustomerAndSendMessage(BookingGroupViewModel groupViewModel)
        {
            var customers = _context.CustomerBookings.Where(x => x.Id != Guid.Empty && x.BookingGroupId == groupViewModel.Id && x.CustomerId != null)
                .Include(x => x.Customer).Include(x => x.BookingGroup).Select(x => new NotificationViewModel()
                {
                    PhoneNuber = x.Customer.PhoneNumber,
                    Email = x.Customer.Email,
                    FirstName = x.Customer.FirstName,
                    NewPrice = x.BookingGroup.ExpectedPrice,
                    IsDate = groupViewModel.IsDate,
                    IsPrice = groupViewModel.IsPrice,
                    ExpectedDateOfArrival = groupViewModel.ExpectedDateOfArrival,
                }).ToList();
            if (customers.Any())
            {
                var loggedInUser = Utitily.GetCurrentUser();

                if (loggedInUser != null)
                {
                    if (groupViewModel.SendSMS)
                    {
                        var amount = customers.Count * _generalConfiguration.SmsAmount;
                        var wallet = _context.Wallets.Where(x => x.CompanyBranchId == loggedInUser.CompanyBranchId).FirstOrDefault();
                        if (wallet != null)
                        {
                            if (wallet.Balance > amount)
                            {
                                foreach (var customer in customers)
                                {
                                    wallet.Balance = wallet.Balance - _generalConfiguration.SmsAmount;
                                    CallHangfire(customer);
                                }

                                _context.Wallets.Update(wallet);
                                _context.SaveChanges();
                                _walletHelper.UpdateWalletHistory(groupViewModel, amount, wallet);
                            }
                        }
                    }
                }
                if (groupViewModel.SendEmail)
                {
                    foreach (var customer in customers)
                    {
                        var sendEmail = SendCustomersEmail(customer);
                    }
                }
            }
        }
        public void CallHangfire(NotificationViewModel userDetail)
        {
            BackgroundJob.Enqueue(() => SendCustomerMessageOnPriceIncrement(userDetail));
        }
        public async Task<bool> SendCustomerMessageOnPriceIncrement(NotificationViewModel userDetail)
        {
            var loggedInUser = Utitily.GetCurrentUser();
            var SMSSenderName = "Zeal Vet";
            string message = "";
            if (userDetail.IsDate)
            {
                message = "Dear " + userDetail.FirstName + "," + "\n" +
                  "This is to notify you of a change in date of arrival in your booking with zeal vet." + "\n" +
                  "The new date is now " + userDetail.ExpectedDateOfArrival + "\n" +


                  "Kind regards,\n" +
                  loggedInUser.BaseUrl;
                
            }
            else
            {
                message = "Dear " + userDetail.FirstName + "," + "\n" +
                  "This is to notify you of an increase in price, in your booking with zeal vet." + "\n" +
                  "The new price is now " + userDetail.NewPrice + "\n" +


                  "Kind regards,\n" +
                  loggedInUser.BaseUrl;
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_generalConfiguration.BuckSMSBaseURL);
                var url = "/v2/app/sms?" +
                    "email=" + _generalConfiguration.BuckSMSEmail +
                    "&password=" + _generalConfiguration.BuckSMSPassword +
                    "&message=" + message +
                    "&sender_name=" + SMSSenderName +
                    "&recipients=" + userDetail.PhoneNuber +
                    "&forcednd= " + _generalConfiguration.BuckSMSforcednd;

                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                var otpRespons = JsonConvert.DeserializeObject<Data>(result);
                if (otpRespons.Status == "1")
                {
                    return true;
                }
                return false;
            }
        }

        public bool SendCustomersEmail(NotificationViewModel userDetail)
        {
            if (userDetail != null)
            {
                var loggedInUser = Utitily.GetCurrentUser();
                string message = "";
                string toEmail = userDetail.Email;
                string subject = "Zeal Vet";
                double? newPrice = userDetail.NewPrice;
                if (userDetail.IsDate)
                {
                    message = "Dear " + userDetail.FirstName + "," + "</br> " +
                  "This is to notify you of a change in expected date of arrival in your booking with zeal vet." + "<br/>" +
                  "The new date is now " + userDetail.ExpectedDateOfArrival + "." + "\n" + "<br/>" +


                  "Kind regards,<br/>" +
                     loggedInUser.BaseUrl;
                }
                else
                {
                    message = "Dear " + userDetail.FirstName + "," + "</br> " +
                   "This is to notify you of an increase in price, in  your booking with zeal vet." + "<br/>" +
                   "The new price is now " + newPrice + "." + "\n" + "<br/>" +


                   "Kind regards,<br/>" +
                    loggedInUser.BaseUrl;
                }

                _emailService.SendEmail(toEmail, subject, message);
                return true;
            };
            return false;
        }

        public bool SendStaffEmail(ApplicationUserViewModel userDetail)
        {
            if (userDetail != null)
            {
                var loggedInUser = Utitily.GetCurrentUser();
                string message = "";
                string toEmail = userDetail.Email;
                string subject = "Zeal Vet";

                message = "Dear " + userDetail.FirstName + "," + "</br> " +
                 "This is to notify you, that you have been registered successfully as a Staff with zeal vet." + "<br/>" +

                  "Kind regards,<br/>" +
                  loggedInUser.BaseUrl;

                _emailService.SendEmail(toEmail, subject, message);

                return true;
            };
            return false;
        }

        public bool PasswordResetConfirmation(ApplicationUser applicationUser)
        {
            if (applicationUser.Email != null)
            {
                var loggedInUser = Utitily.GetCurrentUser();
                string toEmail = applicationUser.Email;
                string subject = "Password Reset Successfully";
                string message = "Password reset Successfully, please login with your new details to continue" + "</br>" +
                    "<br/>Need help? We’re here for you." + "<br/>" +
                    "Simply reply to this email to contact us. <br/>" +
                    "<br/>" +

                    "Kind regards,<br/>" +
                    loggedInUser.BaseUrl;
                _emailService.SendEmail(toEmail, subject, message);
                return true;
            };
            return false;
        }

        public async Task<UserVerification> CreateUserToken(string userEmail)
        {
            var user = _context.ApplicationUsers.Where(x => x.Email == userEmail && !x.IsDeactivated).FirstOrDefault();
            if (user != null)
            {
                var userVerification = new UserVerification()
                {
                    UserId = user.Id,
                };
                await _context.AddAsync(userVerification);
                await _context.SaveChangesAsync();
                return userVerification;
            }
            return null;
        }

        public bool PasswordResetLink(ApplicationUser applicationUser, string linkToClick)
        {
            if (applicationUser != null && applicationUser.Email != null)
            {
                var loggedInUser = Utitily.GetCurrentUser();
                string toEmail = applicationUser.Email;
                string subject = "RESET PASSWORD";
                string message = "A password reset has been requested to your VetMan account email, please click on the button below to create a new password <br>" +
                    "<br/>" + "<a style:'border:2px;' href='" + linkToClick + "' target='_blank'>" + "<button style='color:white; background-color:#ff9b44; padding:10px; border:1px;'>Reset Password</button>" + "</a>" +
                    "<br/> or copy the link below to your browser </br>" + linkToClick + "." + "<br/>" +
                    "Please make sure you've entered the email address you registered with."
                    + "</br> Need help? We’re here for you.Simply reply to this email to contact us. <br/>" +
                    "<br/>" +

                    "<b>Kind regards,</b><br/>" +
                   loggedInUser.BaseUrl;
                _emailService.SendEmail(toEmail, subject, message);
                return true;
            };
            return false;
        }

        public void NotifyCompnayOfSubScription(CompanyModule companyModule)
        {
            var loggedInUser = Utitily.GetCurrentUser();
            var SMSSenderName = "Zeal Vet";
            string toEmail = companyModule.Company?.Email;
            string subject = "Zeal Vet";
            string message = "";
            var expirerDate = companyModule.ExpiryDate.ToString("f");

            message = "Dear " + companyModule.Company?.Name + "," + "\n" +
              "This is to notify you that your subscription will expire on" + expirerDate + "\n" +


              "Kind regards,\n" +
              loggedInUser.BaseUrl;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_generalConfiguration.BuckSMSBaseURL);
                var url = "/v2/app/sms?" +
                    "email=" + _generalConfiguration.BuckSMSEmail +
                    "&password=" + _generalConfiguration.BuckSMSPassword +
                    "&message=" + message +
                    "&sender_name=" + SMSSenderName +
                    "&recipients=" + companyModule.Company?.Phone +
                    "&forcednd= " + _generalConfiguration.BuckSMSforcednd;

                var response = client.GetAsync(url).Result;

            }

            message = "Dear " + companyModule.Company?.Name + "," + "</br> " +
                 "This is to notify you that your subscription will expire on" + expirerDate + "<br/>" +


                 "Kind regards,<br/>" +
                 loggedInUser.BaseUrl;
            _emailService.SendEmail(toEmail, subject, message);
        }

        public bool SendSubscriptionMail(PaystackSubscription subscription)
        {
            var user = Utitily.GetCurrentUser();
            if (subscription != null && user != null)
            {
                string toEmail = _generalConfiguration.AdminEmail;
                string subject = "SUBSCRIPTION NOTIFICATION";
                string message = "&#8358;" + subscription.amount + " has been credited to your Account by " + user.FirstName
                    + "Ref No: " + subscription.reference;

                _emailService.SendEmail(toEmail, subject, message);

                string customersEmail = user.Email;
                string header = "SUBSCRIPTION NOTIFICATION";
                string messageCustomers = "<b> REF NO: " + " " + subscription.reference
                    + "<br/> " + " We recieved a desposit of" + " " + "&#8358;" +
                    subscription.amount + ", " + "In to " +
                    "<br/>  " + " <br/>Make sure you  present your reference number for clarification.";
                _emailService.SendEmail(customersEmail, header, messageCustomers);
                return true;
            }
            return false;
        }
    }
}
