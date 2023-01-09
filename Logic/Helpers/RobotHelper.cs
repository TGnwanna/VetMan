using Core.Config;
using Core.Db;
using Core.Models;
using Hangfire;
using Logic.IHelpers;
using Logic.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static Core.Enums.VetManEnums;
using static Core.Models.PaystackResponse;
using Data = Core.Models.Data;

namespace Logic.Helpers
{
    public class RobotHelper : IRobotHelper
    {
        private readonly AppDbContext _context;
        private readonly IEmailHelper _emailHelper;
        private readonly IGeneralConfiguration _generalConfiguration;
        private readonly IEmailService _emailService;

        public RobotHelper(IEmailHelper emailHelper, AppDbContext context, IGeneralConfiguration generalConfiguration, IEmailService emailService)
        {
            _emailHelper = emailHelper;
            _context = context;
            _generalConfiguration = generalConfiguration;
            _emailService = emailService;
        }

        public void Start2()
        {
            var time = _generalConfiguration.SmsNotificationTime;
            RecurringJob.AddOrUpdate(() => StartRobotBy12Am(), time, TimeZoneInfo.Utc);
        }


        public void StartRobotBy12Am()
        {
            var subscribers = _context.VaccineSubscriptions
                     .Where(s => s.Active && !s.Deleted && s.SubscriptionStatus == SubscriptionStatus.Active).Include(x => x.Product).Include(d => d.Customer).ToList();
            if (subscribers.Count() > 0)
            {
                foreach (var subscriber in subscribers)
                {
                    try
                    {
                        VaccineNotification(subscriber);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
        }

        public void VaccineNotification(VaccineSubscription subscriber)
        {
            try
            {
                var companyBranch = _context.CompanyBranches.Where(x => x.Id == subscriber.CompanyBranchId).Include(s => s.Company).FirstOrDefault();
                if (companyBranch != null)
                {
                    var amount = _generalConfiguration.SmsAmount;
                    var wallet = _context.Wallets.Where(x => x.CompanyBranchId == companyBranch.Id && x.Active && !x.Deleted).FirstOrDefault();
                    if (wallet.Balance > amount)
                    {
                        var isCompanyApproval = _context.CompanySettings.Where(x => x.CompanyId == companyBranch.CompanyId && x.VaccineModule).FirstOrDefault();
                        if (isCompanyApproval != null)
                        {
                            if (isCompanyApproval.VaccineModule)
                            {
                                var vaccines = _context.ProductVaccines.Where(x => x.ProductId == subscriber.ProductId)
                                    .Include(f => f.CompanyBranch).Include(f => f.CompanyBranch.Company).ToList();
                                var currentWeek = DateTime.Now.Day - subscriber.DateDelivered.Day;
                                if (vaccines.Count > 0)
                                {
                                    foreach (var vaccine in vaccines.Where(x => x.Week == currentWeek))
                                    {
                                        wallet.Balance = wallet.Balance - amount;
                                        var isSuccessful = SendVaccineNotification(vaccine, subscriber);
                                        if (isSuccessful)
                                        {
                                            _context.Wallets.Update(wallet);
                                            _context.SaveChanges();
                                            UpdateWalletHistory(wallet, amount);
                                        }

                                    }
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SendVaccineNotification(ProductVaccine productVaccine, VaccineSubscription subscriber)
        {
            if (productVaccine != null && subscriber != null)
            {
                if (subscriber.SmsSubscribed)
                {
                    CallHangfire(productVaccine, subscriber);
                }
                if (subscriber.EmailSubscribed)
                {
                    SendEmailVaccineNotification(productVaccine, subscriber);
                }
                return true;
            }
            return false;
        }
        public void CallHangfire(ProductVaccine productVaccine, VaccineSubscription subscriber)
        {
            BackgroundJob.Enqueue(() => SendSMSVaccineNotification(productVaccine, subscriber));
        }

        public bool SendEmailVaccineNotification(ProductVaccine productVaccine, VaccineSubscription subscription)
        {
            if (productVaccine.CompanyBranch != null && subscription.Customer != null)
            {
                string toEmail = subscription?.Customer?.Email;
                string subject = "VACCINE NOTIFICATION";
                string message = "Dear " + subscription?.Customer?.Name + "," + "\n" +

                    "Your " + productVaccine?.Product?.Name + " is due for " + productVaccine?.Name +
                    " Vaccine at " + productVaccine?.CompanyBranch?.Company?.Name + " on the " + DateTime.Now.AddDays(+1).ToString("D") + "\n" +

                  "Kind regards,\n" +
                  "VetMan Group.";
                _emailService.SendEmail(toEmail, subject, message);
                return true;
            };
            return false;
        }

        public async Task<bool> SendSMSVaccineNotification(ProductVaccine productVaccine, VaccineSubscription subscription)
        {
            if (productVaccine.CompanyBranch != null && subscription.Customer != null)
            {
                var SMSSenderName = productVaccine?.CompanyBranch?.Company?.Name;
                string message = "";

                message = "Dear " + subscription?.Customer?.FullName + "," + "\n" +

                    "Your " + productVaccine?.Product?.Name + " is due for " + productVaccine?.Name +
                    " at " + productVaccine?.CompanyBranch?.Company?.Name + " on the " + DateTime.Now.AddDays(+1).ToString("D") + "\n" +

                  "Kind regards,\n" +
                  "VetMan Group.";


                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_generalConfiguration.BuckSMSBaseURL);
                    var url = "/v2/app/sms?" +
                        "email=" + _generalConfiguration.BuckSMSEmail +
                        "&password=" + _generalConfiguration.BuckSMSPassword +
                        "&message=" + message +
                        "&sender_name=" + SMSSenderName +
                        "&recipients=" + subscription?.Customer?.PhoneNumber +
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
            return false;
        }
        public void NotifySubscribers()
        {
            RecurringJob.AddOrUpdate(() => NotifySubscriptionBackgroundJob(), _generalConfiguration.TimeToNotifyCompany, TimeZoneInfo.Utc);
        }
        public void NotifySubscriptionBackgroundJob()
        {
            try
            {
                var companySubscriptions = _context.CompanyModules.Where(x => x.Id != Guid.Empty && x.CompanyId != Guid.Empty && x.SubcriptionStatus == CompanySubcriptionStatus.Active).ToList();
                if (companySubscriptions != null && companySubscriptions.Count > 0)
                {
                    foreach (var companySubscription in companySubscriptions)
                    {
                        var checkForExpireDate = companySubscription.ExpiryDate.Day <= DateTime.Now.Day;
                        if (checkForExpireDate)
                        {
                            companySubscription.SubcriptionStatus = CompanySubcriptionStatus.Expired;
                            _context.CompanyModules.Update(companySubscription);
                            _context.SaveChanges();
                            UpdateCompanySettings(companySubscription);
                        }
                        else
                        {
                            if (companySubscription.ExpiryDate.Day == 10)
                            {
                                _emailHelper.NotifyCompnayOfSubScription(companySubscription);
                            }
                            if (companySubscription.ExpiryDate.Day == 5)
                            {
                                _emailHelper.NotifyCompnayOfSubScription(companySubscription);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string toEmail = _generalConfiguration.DeveloperEmail;
                string subject = "Exception On Subscription Notification";
                string message = ex.Message + " , <br /> This exception message occurred while trying to notify and update user about subscription";
                _emailService.SendEmail(toEmail, subject, message);
                throw;
            }
        }
        public void UpdateCompanySettings(CompanyModule companyModule)
        {
            try
            {
                var companySetting = _context.CompanySettings.Where(x => x.CompanyId == companyModule.CompanyId).FirstOrDefault();
                if (companySetting != null)
                {
                    switch (companyModule.ModuleId)
                    {
                        case CompanySettings.DOCBookingModule:
                            companySetting.DOCBookingModule = false;
                            break;
                        case CompanySettings.VaccineModule:
                            companySetting.VaccineModule = false;
                            break;
                        case CompanySettings.TransactionModule:
                            companySetting.TransactionModule = false;
                            break;
                        case CompanySettings.StoreModule:
                            companySetting.StoreModule = false;
                            break;
                        default:
                            break;
                    }
                    _context.CompanySettings.Update(companySetting);
                    _context.SaveChanges();
                    _emailHelper.NotifyCompnayOfSubScription(companyModule);
                }
            }
            catch (Exception ex)
            {
                string toEmail = _generalConfiguration.DeveloperEmail;
                string subject = "Exception On Company Subscription Notification";
                string message = ex.Message + " , <br /> This exception message occurred while trying to update user company subscription";
                _emailService.SendEmail(toEmail, subject, message);
                throw;
            }
        }

        public void UpdateWalletHistory(Wallet wallet, double amount)
        {
            if (wallet != null && amount > 0)
            {
                var walletHistory = new WalletHistory()
                {
                    Active = true,
                    Date = DateTime.Now,
                    Description = "Vaccine Notification",
                    PayStackResponses = PayStackResponses.Completed,
                    Amount = amount,
                    TransactionType = TransactionType.Debit,
                    WalletId = wallet.Id,
                    CompanyBranchId = wallet.CompanyBranchId,
                    Name = wallet.Name,
                };
                _context.WalletHistories.Add(walletHistory);
                _context.SaveChanges();
            }
        }
    }
}
