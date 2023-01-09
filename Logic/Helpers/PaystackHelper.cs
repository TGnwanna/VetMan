using Core.Config;
using Core.Db;
using Core.Enums;
using Core.Models;
using Core.ViewModels;
using Logic.IHelpers;
using Logic.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using System.Linq.Expressions;
using System.Net;
using static Core.Enums.VetManEnums;
using static Core.Models.PaystackResponse;

namespace Logic.Helpers
{
    public class PaystackHelper : IPaystackHelper
    {
        private RestClient client;
        protected RestRequest request;
        public static string RestUrl = "https://api.paystack.co/";
        static string ApiEndPoint = "";
        private AppDbContext _context;
        private IGeneralConfiguration _generalConfiguration;
        private IEmailService _emailService;
        private readonly IUserHelper _userHelper;

        public PaystackHelper(AppDbContext context, IGeneralConfiguration generalConfiguration, IEmailService emailService)
        {
            client = new RestClient(RestUrl);
            _context = context;
            _generalConfiguration = generalConfiguration;
            _emailService = emailService;
        }

        public PaystackRepsonse MakeOrderPayment(WalletHistoryViewModel wallet)
        {


            try
            {
                PaystackRepsonse paystackRepsonse = null;
                if (wallet.Amount > 0 && wallet.CompanyBranchId != Guid.Empty)
                {
                    var accountWallet = new Wallet();
                    if (wallet.WalletId == Guid.Empty)
                    {
                        accountWallet = _context.Wallets.Where(x => x.CompanyBranchId == wallet.CompanyBranchId).FirstOrDefault();
                        if (accountWallet != null)
                        {
                            wallet.WalletId = accountWallet.Id;
                        }

                    }
                    if (wallet.WalletId != Guid.Empty)
                    {
                        var createHistory = new WalletHistory()
                        {
                            WalletId = wallet.WalletId,
                            Amount = wallet.Amount,
                            Active = true,
                            CompanyBranchId = wallet.CompanyBranchId,
                            Deleted = false,
                            Description = "funding",
                            TransactionType = VetManEnums.TransactionType.Credit,
                            Date = DateTime.Now,
                            PayStackResponses = VetManEnums.PayStackResponses.InProgress,
                        };
                        _context.Add(createHistory);
                        _context.SaveChanges();
                        wallet.Id = createHistory.Id;

                        var loggedInUser = Utitily.GetCurrentUser();
                        if (loggedInUser.Id == null)
                        {
                            loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName)?.Result;

                        }
                        var referenceId = wallet.Id.ToString().Replace("-", "");
                        long milliseconds = DateTime.Now.Ticks;
                        string testid = milliseconds.ToString();
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        ApiEndPoint = "/transaction/initialize";
                        request = new RestRequest(ApiEndPoint, Method.Post);
                        request.AddHeader("accept", "application/json");
                        request.AddHeader("Authorization", "Bearer " + _generalConfiguration.PayStakApiKey);
                        request.AddParameter("reference", referenceId);
                        var total = wallet.Amount;
                        request.AddParameter("amount", total * 100);

                        List<CustomeField> myCustomfields = new List<CustomeField>();
                        CustomeField nameCustomeField = new CustomeField()
                        {
                            display_name = "Email",
                            variable_name = "Email",
                            value = loggedInUser.Email,
                        };
                        myCustomfields.Add(nameCustomeField);


                        Dictionary<string, List<CustomeField>> metadata = new Dictionary<string, List<CustomeField>>();
                        metadata.Add("custom_fields", myCustomfields);
                        var serializedMetadata = JsonConvert.SerializeObject(metadata);
                        request.AddParameter("metadata", serializedMetadata);
                        request.AddParameter("email", loggedInUser.Email);
                        var serializedRequest = JsonConvert.SerializeObject(request, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                        var result = client.ExecuteAsync(request).Result;
                        if (result.StatusCode == HttpStatusCode.OK)
                        {
                            paystackRepsonse = JsonConvert.DeserializeObject<PaystackRepsonse>(result.Content);
                        }
                    }

                }
                return paystackRepsonse;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<PaystackRepsonse> VerifyPayment(Paystack payment)
        {
            PaystackRepsonse paystackRepsonse = null;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ApiEndPoint = "/transaction/verify/" + payment.reference;
                request = new RestRequest(ApiEndPoint, Method.Get);
                request.AddHeader("accept", "application/json");
                request.AddHeader("Authorization", "Bearer " + _generalConfiguration.PayStakApiKey);
                var result = client.ExecuteAsync(request).Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    paystackRepsonse = JsonConvert.DeserializeObject<PaystackRepsonse>(result.Content);
                    var payStack = UpdateResponse(paystackRepsonse);

                }
                return paystackRepsonse;
            }
            catch (Exception ex)
            {
                var toEmail = _generalConfiguration.AdminEmail;
                var subject = "Paystack Main Payment Verification Exception on VetMan";
                _emailService.SendEmail(toEmail, subject, ex.Message);
                throw;
            }
        }


        public Paystack UpdateResponse(PaystackRepsonse PaystackRepsonse)
        {
            try
            {

                //Expression<Func<Paystack, bool>> selector = ;
                Paystack _paystackEntity = _context.Paystacks.Where(p => p.reference == PaystackRepsonse.data.reference)
                                            ?.Include(s => s.WalletHistory)?.Include(x => x.GuestBooking)?.OrderBy(s => s.amount)?.FirstOrDefault();
                if (_paystackEntity != null)
                {


                    _paystackEntity.bank = PaystackRepsonse.data.authorization.bank;
                    _paystackEntity.brand = PaystackRepsonse.data.authorization.brand;
                    _paystackEntity.card_type = PaystackRepsonse.data.authorization.card_type;
                    _paystackEntity.channel = PaystackRepsonse.data.channel;
                    _paystackEntity.country_code = PaystackRepsonse.data.authorization.country_code;
                    _paystackEntity.currency = PaystackRepsonse.data.currency;
                    _paystackEntity.domain = PaystackRepsonse.data.domain;
                    _paystackEntity.exp_month = PaystackRepsonse.data.authorization.exp_month;
                    _paystackEntity.exp_year = PaystackRepsonse.data.authorization.exp_year;
                    _paystackEntity.fees = PaystackRepsonse.data.fees.ToString();
                    _paystackEntity.gateway_response = PaystackRepsonse.data.gateway_response;
                    _paystackEntity.ip_address = PaystackRepsonse.data.ip_address;
                    _paystackEntity.last4 = PaystackRepsonse.data.authorization.last4;
                    _paystackEntity.message = PaystackRepsonse.message;
                    _paystackEntity.reference = PaystackRepsonse.data.reference;
                    _paystackEntity.reusable = PaystackRepsonse.data.authorization.reusable;
                    _paystackEntity.signature = PaystackRepsonse.data.authorization.signature;
                    _paystackEntity.status = PaystackRepsonse.data.status;
                    _paystackEntity.transaction_date = PaystackRepsonse.data.transaction_date;

                    _context.Update(_paystackEntity);
                    _context.SaveChanges();
                    if (_paystackEntity.WalletHistoryId != null)
                    {
                        var wallet = _context.Wallets.Where(x => x.Id == _paystackEntity.WalletHistory.WalletId).FirstOrDefault();
                        if (wallet != null)
                        {
                            var payStackRate = ((Convert.ToInt32(_paystackEntity.amount) * 1.5) / 100) + 100;
                            var result = Convert.ToInt32(_paystackEntity.amount) - payStackRate;
                            var balance = wallet.Balance + result;
                            wallet.Balance = balance;
                            _context.Wallets.Update(wallet);
                            _paystackEntity.WalletHistory.PayStackResponses = VetManEnums.PayStackResponses.Completed;
                            _paystackEntity.WalletHistory.Description = "Funded";
                            _context.WalletHistories.Update(_paystackEntity.WalletHistory);
                            _context.SaveChanges();
                        }
                    }
                }
                return _paystackEntity;
            }
            catch (Exception ex)
            {
                var toEmail = _generalConfiguration.AdminEmail;
                var subject = "Paystack Updating Response Exception on VetMan";
                _emailService.SendEmail(toEmail, subject, ex.Message);
                throw;
            }
        }


        public Paystack GetBy(string reference)
        {
            try
            {
                return _context.Paystacks.Where(a => a.WalletHistory.WalletId.ToString() == reference).LastOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public WalletHistory ValidatePayment(string reference)
        {
            try
            {
                var details = _context.Paystacks.Where(a => a.WalletHistory.WalletId.ToString() == reference).LastOrDefault();

                if (details != null && details.status != null && details.status.Contains("success") && (details.gateway_response.Contains("Approved") || details.gateway_response.Contains("Transaction Successful") || details.gateway_response.Contains("Successful") || details.gateway_response.Contains("Payment successful") || details.gateway_response.Contains("success")) && details.domain == "live")
                {

                    return details.WalletHistory;
                }

                return null;
            }
            catch (Exception ex)
            {
                var toEmail = _generalConfiguration.AdminEmail;
                var subject = "Paystack validating payment Step2 Exception on Ecoscience";
                _emailService.SendEmail(toEmail, subject, ex.Message);
                throw;
            }
        }


        public DateTime ConvertToDate(string date)
        {
            DateTime newDate = new DateTime();
            try
            {
                string[] dateSplit = date.Split('-');
                newDate = new DateTime(Convert.ToInt32(dateSplit[0]), Convert.ToInt32(dateSplit[1]), Convert.ToInt32(dateSplit[2]));
            }
            catch (Exception)
            {
                throw;
            }

            return newDate;
        }

        public string GeneratePaymentParameterForGuest(GuestBooking guestBooking)
        {
            try
            {
                PaystackRepsonse paystackRepsonse = null;
                var referenceId = guestBooking.Id.ToString().Replace("-", "");
                long milliseconds = DateTime.Now.Ticks;
                string testid = milliseconds.ToString();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ApiEndPoint = "/transaction/initialize";
                request = new RestRequest(ApiEndPoint, Method.Post);
                request.AddHeader("accept", "application/json");
                request.AddHeader("Authorization", "Bearer " + _generalConfiguration.PayStakApiKey);
                request.AddParameter("reference", referenceId);
                request.AddParameter("amount", guestBooking.TotalPrice * 100);

                List<CustomeField> myCustomfields = new List<CustomeField>();
                CustomeField nameCustomeField = new CustomeField()
                {
                    display_name = "Email",
                    variable_name = "Email",
                    value = guestBooking.Email,
                };
                myCustomfields.Add(nameCustomeField);
                Dictionary<string, List<CustomeField>> metadata = new Dictionary<string, List<CustomeField>>();
                metadata.Add("custom_fields", myCustomfields);
                var serializedMetadata = JsonConvert.SerializeObject(metadata);
                request.AddParameter("metadata", serializedMetadata);
                request.AddParameter("email", guestBooking.Email);
                var serializedRequest = JsonConvert.SerializeObject(request, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                var result = client.ExecuteAsync(request).Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    paystackRepsonse = JsonConvert.DeserializeObject<PaystackRepsonse>(result.Content);
                    if (paystackRepsonse != null)
                    {
                        Paystack paystack = new Paystack();
                        paystack.GuestBooking = guestBooking;
                        paystack.authorization_url = paystackRepsonse.data.authorization_url;
                        paystack.access_code = paystackRepsonse.data.access_code;
                        paystack.GuestBookingId = guestBooking.Id;
                        paystack.amount = (decimal?)guestBooking.TotalPrice;
                        paystack.transaction_date = DateTime.UtcNow;
                        paystack.reference = referenceId;
                        _context.Paystacks.Add(paystack);
                        _context.SaveChangesAsync();
                        return paystackRepsonse.data.authorization_url;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                string toEmail = _generalConfiguration.DeveloperEmail;
                string subject = "Exception On Guest Payment";
                string message = ex.Message + " , <br /> This exception message occurred while guest is trying to pay for booking";
                _emailService.SendEmail(toEmail, subject, message);
                throw;
            }
        }

        public PaystackRepsonse SubscriptionPayment(double amount, Guid? id)
        {
            try
            {
                var loggedInUser = Utitily.GetCurrentUser();
                if (loggedInUser.Id == null)
                {
                    loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName)?.Result;
                }
                PaystackRepsonse paystackRepsonse = null;           
                var referenceId = Guid.NewGuid().ToString().Replace("-", "");
                long milliseconds = DateTime.Now.Ticks;
                string testid = milliseconds.ToString();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ApiEndPoint = "/transaction/initialize";
                request = new RestRequest(ApiEndPoint, Method.Post);
                request.AddHeader("accept", "application/json");
                request.AddHeader("Authorization", "Bearer " + _generalConfiguration.PayStakApiKey);
                request.AddParameter("reference", referenceId);
                double total = amount;
                request.AddParameter("amount", total * 100);

                List<CustomeField> myCustomfields = new List<CustomeField>();
                CustomeField nameCustomeField = new CustomeField()
                {
                    display_name = "Email",
                    variable_name = "Email",
                    value = loggedInUser?.Email,
                };
                myCustomfields.Add(nameCustomeField);


                Dictionary<string, List<CustomeField>> metadata = new Dictionary<string, List<CustomeField>>();
                metadata.Add("custom_fields", myCustomfields);
                var serializedMetadata = JsonConvert.SerializeObject(metadata);
                request.AddParameter("metadata", serializedMetadata);
                request.AddParameter("email", loggedInUser.Email);
                var serializedRequest = JsonConvert.SerializeObject(request, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                var result = client.ExecuteAsync(request).Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    paystackRepsonse = JsonConvert.DeserializeObject<PaystackRepsonse>(result.Content);
                }
                return paystackRepsonse;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<PaystackRepsonse> VerifyPayment(PaystackSubscription payment)
        {
            PaystackRepsonse paystackRepsonse = null;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ApiEndPoint = "/transaction/verify/" + payment.reference;
                request = new RestRequest(ApiEndPoint, Method.Get);
                request.AddHeader("accept", "application/json");
                request.AddHeader("Authorization", "Bearer " + _generalConfiguration.PayStakApiKey);
                var result =  client.ExecuteAsync(request).Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    paystackRepsonse = JsonConvert.DeserializeObject<PaystackRepsonse>(result.Content);
                    var payStack = UpdateSubscriptionResponse(paystackRepsonse).Result;

                }
                return paystackRepsonse;
            }
            catch (Exception ex)
            {
                var toEmail = _generalConfiguration.AdminEmail;
                var subject = "Paystack Main Payment Verification Exception on VetMan";
                _emailService.SendEmail(toEmail, subject, ex.Message);
                throw;
            }
        }


        public async Task<PaystackSubscription> UpdateSubscriptionResponse(PaystackRepsonse PaystackRepsonse)
        {
            try
            {
                PaystackSubscription _paystackEntity = _context.PaystackSubscriptions.Where(p => p.reference == PaystackRepsonse.data.reference)?.FirstOrDefault();

                if (_paystackEntity != null)
                {
                    _paystackEntity.bank = PaystackRepsonse.data.authorization.bank;
                    _paystackEntity.brand = PaystackRepsonse.data.authorization.brand;
                    _paystackEntity.card_type = PaystackRepsonse.data.authorization.card_type;
                    _paystackEntity.channel = PaystackRepsonse.data.channel;
                    _paystackEntity.country_code = PaystackRepsonse.data.authorization.country_code;
                    _paystackEntity.currency = PaystackRepsonse.data.currency;
                    _paystackEntity.domain = PaystackRepsonse.data.domain;
                    _paystackEntity.exp_month = PaystackRepsonse.data.authorization.exp_month;
                    _paystackEntity.exp_year = PaystackRepsonse.data.authorization.exp_year;
                    _paystackEntity.fees = PaystackRepsonse.data.fees.ToString();
                    _paystackEntity.gateway_response = PaystackRepsonse.data.gateway_response;
                    _paystackEntity.ip_address = PaystackRepsonse.data.ip_address;
                    _paystackEntity.last4 = PaystackRepsonse.data.authorization.last4;
                    _paystackEntity.message = PaystackRepsonse.message;
                    _paystackEntity.reusable = PaystackRepsonse.data.authorization.reusable;
                    _paystackEntity.signature = PaystackRepsonse.data.authorization.signature;
                    _paystackEntity.status = PaystackRepsonse.data.status;
                    _paystackEntity.transaction_date = PaystackRepsonse.data.transaction_date;

                    _context.Update(_paystackEntity);
                    _context.SaveChanges();
                    var utility = new Utitily();
                    var idList = utility.moduleSubscription();
                     
                    if (idList.Count > 0)
                    {
                        foreach (var id in idList)
                        {
                            var initialSub = _context.CompanyModules.Where(t => t.ModuleCostId == id 
                                        && t.CompanyId == _paystackEntity.CompanyId && t.SubcriptionStatus == CompanySubcriptionStatus.Active).FirstOrDefault();
                            if (initialSub !=  null)
                            {
                                initialSub.SubcriptionStatus = CompanySubcriptionStatus.Expired;
                                _context.CompanyModules.Update(initialSub);
                            }
                        }
                        _context.SaveChanges();
                    }

                    var modules = _context.CompanyModules.Where(x => x.CompanyId == _paystackEntity.CompanyId && x.StartDate.Date == DateTime.Now.Date &&
                                            x.SubcriptionStatus == CompanySubcriptionStatus.Pending).ToList();
                    if (modules.Count > 0)
                    {
                        foreach (var module in modules)
                        {                         
                            module.SubcriptionStatus = CompanySubcriptionStatus.Active;
                            _context.CompanyModules.Update(module);
                        }
                        _context.SaveChanges();
                    }
                    
                }
                return _paystackEntity;
            }
            catch (Exception ex)
            {
                var toEmail = _generalConfiguration.AdminEmail;
                var subject = "Paystack Updating Response Exception on VetMan";
                _emailService.SendEmail(toEmail, subject, ex.Message);
                throw;
            }
        }
    }
}
