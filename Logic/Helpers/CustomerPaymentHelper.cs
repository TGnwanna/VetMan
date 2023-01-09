using AutoMapper;
using Core.Config;
using Core.Db;
using Core.Models;
using Core.ViewModels;
using Hangfire;
using Logic.IHelpers;
using Logic.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using static Core.Enums.VetManEnums;

namespace Logic.Helpers
{
    public class CustomerPaymentHelper : ICustomerPaymentHelper
    {
        private readonly IEmailHelper _emailHelper;
        private readonly IGeneralConfiguration _generalConfiguration;
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IUserHelper _userHelper;
        private UserManager<ApplicationUser> _userManager;

        public CustomerPaymentHelper(AppDbContext context, UserManager<ApplicationUser> userManager, IUserHelper userHelper, IEmailService emailService, IMapper mapper,
            IEmailHelper emailHelper, IGeneralConfiguration generalConfiguration)
        {
            _context = context;
            _emailHelper = emailHelper;
            _generalConfiguration = generalConfiguration;
            _emailService = emailService;
            _userHelper = userHelper;
            _userManager = userManager;
        }

        /// <summary>
        /// Fetches all customer in a particular booking group using the booking group id
        /// </summary>
        /// <param name="bookingGroupId"></param>
        /// <returns>
        /// All Customers in a booking group
        /// </returns>
        public CustomerPaymentGeneralViewModel GetCustomersInGroup(Guid bookingGroupId)
        {
            var customerPaymentGeneralViewModel = new CustomerPaymentGeneralViewModel();
            var loggedInUser = Utitily.GetCurrentUser();
            if (loggedInUser.Id == null)
            {
                loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

            }
            if (loggedInUser != null)
            {
                if (bookingGroupId != Guid.Empty)
                {
                    var bookingGroup = _context.BookingGroups.Where(x => x.Id == bookingGroupId && x.Active && x.CompanyBranchId == loggedInUser.CompanyBranchId).FirstOrDefault();
                    var customers = _context.CustomerBookings
                   .Where(x => x.Id != Guid.Empty && x.BookingGroupId == bookingGroupId && x.CustomerId != null)
                   .Include(x => x.Customer).Include(x => x.BookingGroup).Select(x => new CustomerBookingViewModel()
                   {
                       Id = x.Id,
                       TotalPaid = x.TotalPaid,
                       Status = x.Status.ToString(),
                       BookingDate = x.BookingDate.ToString("d"),
                       QuantityBooked = x.QuantityBooked,
                       CustomerId = x.CustomerId,
                       BookingGroupId = x.BookingGroupId,
                       CustomersFirstName = x.Customer.FirstName,
                       ExpectedPrice = x.BookingGroup.ExpectedPrice,
                       CustomersEmail = x.Customer.Email,
                       CustomersPhoneNumber = x.Customer.PhoneNumber,
                       CustomersLastName = x.Customer.LastName,
                       CustomersFullName = x.Customer.FullName,
                       Balance = x.Balance,
                       CustomersAddress = x.Customer.Address,
                       BookingGroupName = x.BookingGroup.Name,

                   }).ToList();
                    if (bookingGroup != null)
                    {
                        customerPaymentGeneralViewModel.GroupName = bookingGroup.Name;
                        customerPaymentGeneralViewModel.GroupId = bookingGroup.Id;
                    }
                    if (customers.Any())
                    {
                        customerPaymentGeneralViewModel.CustomerBookingViewModel = customers;
                        customerPaymentGeneralViewModel.CustomerCount = customers.Count;
                        customerPaymentGeneralViewModel.SmsCost = _generalConfiguration.SmsAmount;

                        return customerPaymentGeneralViewModel;
                    }
                    return customerPaymentGeneralViewModel;
                }
            }
            return customerPaymentGeneralViewModel;
        }
        /// <summary>
        /// Gets all customers payment history using the customerBookingId and returns all customer
        /// </summary>
        /// <param name="customerBookingId"></param>
        /// <returns>
        /// Customer booking History
        /// </returns>
        public CustomerPaymentGeneralViewModel GetCustomerBookingPayment(Guid customerBookingId, ApplicationUser loggedInUser)
        {
            var customerPaymentGeneralViewModel = new CustomerPaymentGeneralViewModel();
            if (customerBookingId != Guid.Empty && loggedInUser != null)
            {
                var customerBookingPayments = _context.CustomerBookingPayments
               .Where(x => x.CustomerBookingId == customerBookingId && x.Active && x.CompanyBranchId == loggedInUser.CompanyBranchId).Include(x => x.UpdatedBy).
               Select(s => new CustomerBookingPaymentViewModel()
               {
                   Id = s.Id,
                   UpdatedByUserName = s.UpdatedBy.FullName,
                   CustomerBookingId = customerBookingId,
                   DatePaid = s.DatePaid.ToString("D"),
                   Amount = s.Amount,
                   Status = s.CustomerBooking.Status.ToString(),
                   CustomersFullName = s.CustomerBooking.Customer.FullName,
                   CustomersEmail = s.CustomerBooking.Customer.Email,
                   CustomersPhoneNumber = s.CustomerBooking.Customer.PhoneNumber,
                   UpdatedByUserId = s.UpdatedByUserId,
                   CustomersId = s.CustomerBooking.CustomerId,

               }).ToList();
                customerPaymentGeneralViewModel.CustomerFullName = customerBookingPayments.FirstOrDefault()?.CustomersFullName;
                customerPaymentGeneralViewModel.UpdatedByUserId = loggedInUser.Id;
                customerPaymentGeneralViewModel.CustomerBookingId = customerBookingId;
                if (customerBookingPayments.Count > 0)
                {
                    customerPaymentGeneralViewModel.CustomerBookingPaymentViewModel = customerBookingPayments;
                    return customerPaymentGeneralViewModel;
                }
                return customerPaymentGeneralViewModel;
            }
            return customerPaymentGeneralViewModel;
        }

        public CustomerBookingPayment AddPayment(CustomerBookingPaymentViewModel paymentDetails)
        {
            var loggedInUser = Utitily.GetCurrentUser();
            if (loggedInUser.Id == null)
            {
                loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

            }
            if (loggedInUser != null)
            {
                if (paymentDetails != null)
                {
                    var payment = new CustomerBookingPayment();
                    payment.CompanyBranchId = loggedInUser.CompanyBranchId;
                    payment.DatePaid = DateTime.Parse(paymentDetails.DatePaid);
                    payment.UpdatedByUserId = paymentDetails.UpdatedByUserId;
                    payment.CustomerBookingId = paymentDetails.CustomerBookingId;
                    payment.Active = true;
                    payment.Deleted = false;
                    payment.Amount = paymentDetails.Amount;
                    _context.CustomerBookingPayments.Add(payment);
                    _context.SaveChanges();
                    UpdateCustomersBookingPayment(payment.Id);
                    return payment;
                }
            }
            return null;
        }

        public BookingGroup UpdatePrice(BookingGroupViewModel paymentDetails)
        {
            if (paymentDetails.Id != Guid.Empty)
            {
                var bookingGroup = _context.BookingGroups.Where(x => x.Id == paymentDetails.Id && x.ExpectedPrice != null).FirstOrDefault();
                if (bookingGroup != null)
                {
                    bookingGroup.Id = paymentDetails.Id;
                    bookingGroup.ExpectedPrice = paymentDetails.ExpectedPrice;
                    bookingGroup.ExpectedCostPrice = paymentDetails.ExpectedPrice;
                    bookingGroup.ProductId = paymentDetails.ProductId;
                    _context.BookingGroups.Update(bookingGroup);
                    _context.SaveChanges();
                    return bookingGroup;
                }
            }
            return null;
        }

        public void UpdateGroupExpectedPayment(Guid groupId)
        {
            var bookingGroup = _context.BookingGroups.Where(x => x.Id == groupId).FirstOrDefault();
            if (bookingGroup != null)
            {
                var customerBookings = _context.CustomerBookings.Where(x => x.Id != Guid.Empty && x.BookingGroupId == bookingGroup.Id && x.QuantityBooked != null).ToList();
                if (bookingGroup != null)
                {
                    foreach (var customer in customerBookings)
                    {
                        var quantityBooked = customer.QuantityBooked;
                        var expectedPrice = bookingGroup.ExpectedPrice;
                        var totalPaymentTobeMade = expectedPrice * quantityBooked;
                        var totalPaid = customer.TotalPaid;
                        customer.Active = true;
                        customer.Deleted = false;
                        var balance = totalPaymentTobeMade - totalPaid;
                        customer.Balance = balance;
                        _context.CustomerBookings.Update(customer);
                        _context.SaveChanges();
                    }
                }
            }
        }
        public void UpdateGroupPaymenWithHangFire(Guid bookingId)
        {
            BackgroundJob.Enqueue(() => UpdateGroupExpectedPayment(bookingId));
        }
        public void UpdateCustomersBookingPayment(Guid customersBookingPaymentId)
        {
            var customersBookingPayment = _context.CustomerBookingPayments.Where(x => x.Id == customersBookingPaymentId && x.UpdatedByUserId != null).FirstOrDefault();
            if (customersBookingPayment != null)
            {
                var amountPaid = customersBookingPayment.Amount;
                var customerBookings = _context.CustomerBookings.Where(x => x.Id == customersBookingPayment.CustomerBookingId && x.TotalPaid != null && x.BookingGroupId != Guid.Empty).FirstOrDefault();
                {
                    var bookingGroup = _context.BookingGroups.Where(x => x.Id == customerBookings.BookingGroupId && x.ExpectedPrice != null).FirstOrDefault();
                    {
                        if (bookingGroup != null)
                        {
                            var quantityBooked = customerBookings.QuantityBooked;
                            var expectedPrice = bookingGroup.ExpectedPrice;
                            var totalPaymentTobeMade = expectedPrice * quantityBooked;
                            var totalPaid = customersBookingPayment.Amount + customerBookings.TotalPaid;
                            var balance = totalPaymentTobeMade - totalPaid;
                            customerBookings.Balance = balance;
                            customerBookings.TotalPaid = totalPaid;
                            _context.CustomerBookings.Update(customerBookings);
                            _context.SaveChanges();
                        }
                    }
                }
            }
        }

        public BookingGroup UpdateDate(BookingGroupViewModel dateDetails)
        {
            if (dateDetails.Id != Guid.Empty)
            {
                var bookingGroup = _context.BookingGroups.Where(x => x.Id == dateDetails.Id).FirstOrDefault();
                if (bookingGroup != null)
                {
                    dateDetails.ExpectedDateOfArrivalD = DateTime.Parse(dateDetails.ExpectedDateOfArrival);
                    bookingGroup.Id = bookingGroup.Id;
                    bookingGroup.ExpectedDateOfArrival = dateDetails.ExpectedDateOfArrivalD;
                    _context.BookingGroups.Update(bookingGroup);
                    _context.SaveChanges();
                    return bookingGroup;
                }
            }
            return null;
        }

        public CustomerBookingViewModel GetGroupSummary(Guid groupId)
        {
            var customerBookingView = new CustomerBookingViewModel();
            if (groupId != Guid.Empty)
            {
                var bookingGroup = _context.BookingGroups.Where(x => x.Id == groupId).FirstOrDefault();
                if (bookingGroup != null)
                {
                    var customerBookings = _context.CustomerBookings.Where(x => x.Id != Guid.Empty && x.BookingGroupId == groupId).ToList();
                    var quantityCancel = customerBookings.Where(x => x.Status == Status.Canceled).Sum(x => x.QuantityBooked);
                    var balanceCancel = customerBookings.Where(x => x.Status == Status.Canceled).Sum(x => x.Balance);
                    var amountCancel = customerBookings.Where(x => x.Status == Status.Canceled).Sum(x => x.TotalPaid);
                    var amountPaid = customerBookings.Sum(x => x.TotalPaid);
                    var deliver = customerBookings.Where(x => x.Status == Status.Delivered).Sum(x => x.QuantityBooked);
                    var balanceNotCancel = customerBookings.Sum(x => x.Balance);
                    if (customerBookings != null && customerBookings.Count > 0)
                    {

                        var mortalityNumber = bookingGroup.MotalityRecorded;
                        var quantityLeft = (bookingGroup.QuantityArrived - mortalityNumber) - deliver;
                        var expectedPrice = bookingGroup.ExpectedPrice;
                        var expectedCostPrice = bookingGroup.ExpectedCostPrice;
                        var quantitySold = customerBookings.Where(x => x.Status == Status.Delivered).Sum(s => s.QuantityBooked);
                        var totalPaid = amountPaid - amountCancel;
                        var quantityBooked = customerBookings.Sum(x => x.QuantityBooked) - quantityCancel;
                        var totalBooked = customerBookings.Sum(x => x.QuantityBooked);
                        var balance = balanceNotCancel - balanceCancel;
                        var supplyPrice = bookingGroup.SupplyPrice;
                        //var quantityLeft = (bookingGroup.QuantityArrived - mortalityNumber) - bookingGroup.QuantitySold;
                        var sold = bookingGroup.QuantityArrived - (mortalityNumber + quantityLeft);
                        var totalDelivereds = customerBookings.Select(x => x.BookingGroup?.QuantitySold).Sum();
                        var totalAmountBought = Convert.ToDouble(bookingGroup.QuantityArrived) * Convert.ToDouble(bookingGroup.ExpectedPrice);
                        var totalAmountSold = Convert.ToDouble(quantitySold) * Convert.ToDouble(bookingGroup.SupplyPrice);
                        customerBookingView.BookingGroupName = bookingGroup.Name;
                        customerBookingView.BookingGroupId = bookingGroup.Id;
                        customerBookingView.TotalBooked = Convert.ToInt32(totalBooked);
                        customerBookingView.QuantityLeft = Convert.ToInt32(quantityLeft);
                        customerBookingView.QuantityArrived = Convert.ToInt32(bookingGroup.QuantityArrived);
                        customerBookingView.QuantitySold = Convert.ToInt32(quantitySold);
                        customerBookingView.TotalCancelled = Convert.ToInt32(quantityCancel);
                        customerBookingView.TotalPaid = totalPaid;
                        customerBookingView.MotalityRecorded = mortalityNumber;
                        customerBookingView.PotentialProfit = Convert.ToDouble(quantityLeft) * Convert.ToDouble(bookingGroup.ExpectedCostPrice);
                        customerBookingView.QuantityBooked = quantityBooked;
                        customerBookingView.Balance = balance;
                        customerBookingView.SupplyPrice = Convert.ToInt32(supplyPrice);
                        customerBookingView.ExpectedCostPrice = expectedCostPrice;
                        customerBookingView.ExpectedPrice = bookingGroup.ExpectedPrice;
                        customerBookingView.TotalDelivered = Convert.ToInt32(deliver);
                        customerBookingView.ProfitMade = (quantitySold * bookingGroup.ExpectedCostPrice) - (bookingGroup.QuantityArrived * bookingGroup.SupplyPrice);


                        return customerBookingView;
                    }
                    return customerBookingView;
                }
            }
            return null;
        }

        public async Task<bool> CreateCompany(CompanyViewModel companyViewModel)
        {
            var createUser = await _userHelper.CreateCompanyAdminUser(companyViewModel).ConfigureAwait(false);
            if (createUser != null)
            {
                var company = new Company()
                {
                    Name = companyViewModel.Name,
                    Address = companyViewModel.CompanyAddress,
                    DateCreated = DateTime.Now,
                    Email = companyViewModel.Email,
                    Phone = companyViewModel.Phone,
                    Mobile = companyViewModel.Mobile,
                    Active = true,
                    Deleted = false,
                    CreatedById = createUser.Id,
                };
                await _context.Companies.AddAsync(company).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                CompanyFreeDays(company.Id);
                var createBranch = CreateMainBranchForCompany(company.Id, createUser.Id);
                if (createBranch)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CreateMainBranchForCompany(Guid companyId, string companyAdminId)
        {
            if (companyId != Guid.Empty)
            {
                var company = _context.Companies.Where(x => x.Id == companyId && x.Name != null && x.Address != null && x.CreatedById == companyAdminId).FirstOrDefault();
                if (company != null)
                {
                    var companyBranch = new CompanyBranch();
                    companyBranch.Name = "Main Branch";
                    companyBranch.Active = true;
                    companyBranch.Deleted = false;
                    companyBranch.DateCreated = DateTime.Now;
                    companyBranch.CompanyId = company.Id;
                    companyBranch.Address = company.Address;
                    companyBranch.IsMainBranch = true;
                    _context.CompanyBranches.Add(companyBranch);
                    _context.SaveChanges();
                    CreateWalletForCompanyBranch(companyBranch.Id);
                    UpdateUserBranch(companyBranch.Id, companyAdminId);
                    return true;
                }
            }
            return false;
        }

        public List<CompanyViewModel> GetAllCompanies()
        {
            var companyViewModel = new List<CompanyViewModel>();
            var allCompanies = _context.Companies.Where(x => x.Id != Guid.Empty && x.Active && x.CreatedById != null).Include(x => x.CreatedBy)
                .Select(x => new CompanyViewModel
                {
                    Name = x.Name,
                    Id = x.Id,
                    AdminFullName = x.CreatedBy.FullName,
                    DateCreated = x.DateCreated,
                    Phone = x.Phone,
                    Email = x.Email,
                    Mobile = x.Mobile,
                    CreatedById = x.CreatedById,
                }).ToList();
            if (allCompanies.Count > 0 && allCompanies != null)
            {
                return allCompanies;
            }
            return companyViewModel;
        }

        public void UpdateUserBranch(Guid branchId, string userId)
        {
            var user = _context.ApplicationUsers.Where(x => x.Id == userId && !x.IsDeactivated).Include(x => x.CompanyBranch).FirstOrDefault();
            if (user != null)
            {
                user.CompanyBranchId = branchId;
                _context.ApplicationUsers.Update(user);
                _context.SaveChanges();
            }
        }


        public void CreateWalletForCompanyBranch(Guid branchId)
        {
            var wallet = new Wallet()
            {
                CompanyBranchId = branchId,
                Name = "Utilty",
                Active = true,
                Balance = 0.0,
                DateCreated = DateTime.Now,
                Deleted = false,
            };
            _context.Wallets.Add(wallet);
            _context.SaveChanges();
        }

        public Guid? UpdateCustomerBookingStatus(CustomerBooking customerBooking, SubscriptionOptionsViewModel details)
        {
            if (customerBooking != null)
            {
                customerBooking.Status = Status.Delivered;
                _context.CustomerBookings.Update(customerBooking);
                _context.SaveChanges();
                if (details.EmailSubscribed == true || details.SmsSubscribed == true)
                    CreateVaccineSubscriber(details, customerBooking);
                return customerBooking.BookingGroupId;
            }
            return Guid.Empty;
        }
        public bool CreateVaccineSubscriber(SubscriptionOptionsViewModel details, CustomerBooking customerBooking)
        {
            try
            {
                var getProduct = _context.BookingGroups.Where(v => v.Id == customerBooking.BookingGroupId).FirstOrDefault();
                var getDCustomerName = _context.Customers.Where(v => v.Id == customerBooking.CustomerId).FirstOrDefault();
                var productId = 0;
                if (getProduct.ProductId != null)
                {
                    var vaccineSubscription = new VaccineSubscription()
                    {
                        SmsSubscribed = details.SmsSubscribed,
                        EmailSubscribed = details.EmailSubscribed,
                        ProductId = (int)getProduct.ProductId,
                        DateDelivered = DateTime.Today,
                        SubscriptionStatus = SubscriptionStatus.Active,
                        CustomerId = (Guid)customerBooking.CustomerId,
                        CompanyBranchId = customerBooking.CompanyBranchId,
                        Name = getDCustomerName.FullName,
                        Active = true,
                        Deleted = false,
                    };
                    _context.VaccineSubscriptions.Add(vaccineSubscription);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public CustomerBooking MoveToNewGroup(CustomerBookingViewModel customerBookingViewModel)
        {
            var loggedInUser = Utitily.GetCurrentUser();
            if (loggedInUser.Id == null)
            {
                loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

            }
            if (loggedInUser != null)
            {
                var customerBookingDetails = _context.CustomerBookings.Where(x => x.Id == customerBookingViewModel.Id && x.Status == Status.Booked && x.Active && x.BookingGroupId != Guid.Empty).Include(x => x.BookingGroup).FirstOrDefault();
                if (customerBookingDetails != null)
                {
                    var newGroup = _context.BookingGroups.Where(x => x.Id == customerBookingViewModel.BookingGroupId && x.CompanyBranchId == loggedInUser.CompanyBranchId && x.Active).FirstOrDefault();
                    if (newGroup != null)
                    {
                        var expectedMoneyToBePaid = customerBookingDetails.QuantityBooked * newGroup.ExpectedPrice;
                        var balance = expectedMoneyToBePaid - customerBookingDetails.TotalPaid;
                        customerBookingDetails.BookingGroupId = newGroup.Id;
                        customerBookingDetails.Balance = balance;
                        customerBookingDetails.BookingDate = DateTime.Now;
                        customerBookingDetails.DateCreated = DateTime.Now;
                        _context.Update(customerBookingDetails);
                        _context.SaveChanges();
                        return customerBookingDetails;
                    }
                }
            }
            return null;
        }
        public void SyncCustmerPayment(Guid bookingGroupId, Guid newGroupId, Guid? companyBranchId)
        {
            BackgroundJob.Enqueue(() => MoveAllUserToNewGroup(bookingGroupId, newGroupId, companyBranchId));
        }
        public void MoveAllUserToNewGroup(Guid bookingGroupId, Guid newGroupId, Guid? companyBranchId)
        {
            var customerBookings = _context.CustomerBookings.Where(x => x.BookingGroupId == bookingGroupId && x.Status == Status.Booked && x.Active && x.CompanyBranchId == companyBranchId).Include(x => x.Customer).ToList();
            if (customerBookings != null)
            {
                var newGroup = _context.BookingGroups.Where(x => x.Id == newGroupId && x.CompanyBranchId == companyBranchId && x.Active).FirstOrDefault();
                if (newGroup != null)
                {
                    foreach (var customer in customerBookings)
                    {
                        var expectedMoneyToBePaid = customer.QuantityBooked * newGroup.ExpectedPrice;
                        var balance = expectedMoneyToBePaid - customer.TotalPaid;
                        customer.BookingGroupId = newGroup.Id;
                        customer.Balance = balance;
                        customer.BookingDate = DateTime.Now;
                        customer.DateCreated = DateTime.Now;
                        _context.Update(customer);
                        _context.SaveChanges();
                    }
                }
            }
        }
        public List<CustomerBooking> GetBookedCustomer(Guid bookingGroupId, Guid? companyBranchId)
        {
            var customers = new List<CustomerBooking>();
            var customerBookings = _context.CustomerBookings.Where(x => x.BookingGroupId == bookingGroupId && x.Status == Status.Booked && x.Active && x.CompanyBranchId == companyBranchId).Include(x => x.Customer).ToList();
            if (customerBookings != null && customerBookings.Count > 0)
            {
                return customerBookings;
            }
            return customers;
        }
        public List<drp> GetAllBookingGroup()
        {
            var loggedInUser = Utitily.GetCurrentUser();
            if (loggedInUser.Id == null)
            {
                loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

            }
            var common = new drp()
            {
                Id = Guid.Empty,
                Name = "-- Select --"
            };
            var bookingGroups = _context.BookingGroups.Where(x => !x.Deleted && x.CompanyBranchId == loggedInUser.CompanyBranchId)
                .Select(x => new drp
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
            bookingGroups.Insert(0, common);
            return bookingGroups;
        }
        public List<VaccineSubscriptionViewModel> SubscribersList()
        {
            var vaccineSubscriptionViewModel = new List<VaccineSubscriptionViewModel>();
            var loggedInUser = Utitily.GetCurrentUser();
            if (loggedInUser.Id == null)
            {
                loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

            }
            if (loggedInUser != null)
            {
                var subscriber = _context.VaccineSubscriptions
                    .Where(s => s.CompanyBranchId == loggedInUser.CompanyBranchId && !s.Deleted)
                    .Include(s => s.Product)
                .Select(s => new VaccineSubscriptionViewModel()
                {
                    Id = s.Id,
                    FullName = s.Name,
                    ProductName = s.Product.Name,
                    SmsSubscribed = s.SmsSubscribed,
                    EmailSubscribed = s.EmailSubscribed,
                    SubscriptionStatus = s.SubscriptionStatus,
                    DateDelivered = s.DateDelivered,
                }).ToList();
                return subscriber;
            }

            return vaccineSubscriptionViewModel;

        }
        public VaccineSubscription GetVaccineSubscriberData(int SubDetails)
        {
            if (SubDetails != null)
            {
                var subToEdit = _context.VaccineSubscriptions.Where(x => x.Id == SubDetails && !x.Deleted).FirstOrDefault();
                if (subToEdit != null)
                    return subToEdit;
            }
            return null;
        }
        public VaccineSubscription EditSubDetails(VaccineSubscriptionViewModel subData)
        {
            if (subData != null)
            {
                var subDetails = _context.VaccineSubscriptions.Where(x => x.Id == subData.Id && x.Active && !x.Deleted).FirstOrDefault();
                if (subDetails != null)
                {
                    var loggedInuser = Utitily.GetCurrentUser();
                    if (loggedInuser.Id == null)
                    {
                        loggedInuser = _userHelper.UpdateSessionAsync(loggedInuser.UserName).Result;

                    }
                    if (loggedInuser != null)
                    {
                        subDetails.SmsSubscribed = subData.SmsSubscribed;
                        subDetails.EmailSubscribed = subData.EmailSubscribed;
                        subDetails.DateDelivered = subData.DateDelivered;
                        _context.Update(subDetails);
                        _context.SaveChanges();
                        return subDetails;
                    }
                }
            }
            return null;
        }
        public VaccineSubscription CancelSubscriber(VaccineSubscriptionViewModel subscriberData)
        {
            if (subscriberData != null)
            {
                var subscriber = _context.VaccineSubscriptions.Where(x => x.Id == subscriberData.Id && x.Active && !x.Deleted).FirstOrDefault();
                if (subscriber != null)
                {
                    subscriber.SubscriptionStatus = Core.Enums.VetManEnums.SubscriptionStatus.Canceled;
                    subscriber.EmailSubscribed = false;
                    subscriber.SmsSubscribed = false;
                    _context.VaccineSubscriptions.Update(subscriber);
                    _context.SaveChanges();
                    return subscriber;
                }
            }
            return null;
        }
        public VaccineSubscription CompletedSubscriber(VaccineSubscriptionViewModel subscriberData)
        {
            if (subscriberData != null)
            {
                var subscriber = _context.VaccineSubscriptions.Where(x => x.Id == subscriberData.Id && x.Active && !x.Deleted).FirstOrDefault();
                if (subscriber != null)
                {
                    subscriber.SubscriptionStatus = Core.Enums.VetManEnums.SubscriptionStatus.Completed;
                    subscriber.EmailSubscribed = false;
                    subscriber.SmsSubscribed = false;
                    _context.VaccineSubscriptions.Update(subscriber);
                    _context.SaveChanges();
                    return subscriber;
                }
            }
            return null;
        }
        public VaccineSubscription DeleteSubscriber(VaccineSubscriptionViewModel subscriberData)
        {
            if (subscriberData != null)
            {
                var subscriber = _context.VaccineSubscriptions.Where(x => x.Id == subscriberData.Id && x.Active && !x.Deleted).FirstOrDefault();
                if (subscriber != null)
                {
                    subscriber.Deleted = true;
                    subscriber.EmailSubscribed = false;
                    subscriber.SmsSubscribed = false;
                    subscriber.Active = false;
                    _context.VaccineSubscriptions.Update(subscriber);
                    _context.SaveChanges();
                    return subscriber;
                }
            }
            return null;
        }
        public List<Product> GetProductDropDown()
        {
            var loggedInUser = Utitily.GetCurrentUser();
            if (loggedInUser.Id == null)
            {
                loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

            }
            var common = new Product()
            {
                Id = 0,
                Name = "-- Select --"
            };
            var products = _context.Products.Where(x => !x.Deleted && x.CompanyBranchId == loggedInUser.CompanyBranchId).ToList();
            products.Insert(0, common);
            return products;
        }
        public List<Customer> GetCustomerDropDown()
        {
            var loggedInUser = Utitily.GetCurrentUser();
            if (loggedInUser.Id == null)
            {
                loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

            }
            var common = new Customer()
            {
                Id = Guid.Empty,
                Name = "-- Select --"
            };
            var customer = _context.Customers.Where(x => !x.Deleted && x.CompanyBranchId == loggedInUser.CompanyBranchId).ToList();
            customer.Insert(0, common);
            return customer;
        }
        public bool CreateCustomerDetails(VaccineSubscriptionViewModel customerDetails)
        {
            var loggedInUser = Utitily.GetCurrentUser();
            if (loggedInUser.Id == null)
            {
                loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

            }
            if (loggedInUser != null)
            {
                var customerValidation = _context.VaccineSubscriptions.Where(c => c.Name == customerDetails.FullName && c.ProductId == customerDetails.ProductId && c.CompanyBranchId == loggedInUser.CompanyBranchId).FirstOrDefault();
                if (customerValidation == null)
                {
                    var subscriberModel = new VaccineSubscription()
                    {
                        CustomerId = customerDetails.CustomerId,
                        ProductId = customerDetails.ProductId,
                        Name = customerDetails.FullName,
                        EmailSubscribed = customerDetails.EmailSubscribed,
                        SmsSubscribed = customerDetails.SmsSubscribed,
                        DateCreated = DateTime.Now,
                        DateDelivered = customerDetails.DateDelivered,
                        SubscriptionStatus = Core.Enums.VetManEnums.SubscriptionStatus.Active,
                        Active = true,
                        Deleted = false,
                        CompanyBranchId = loggedInUser.CompanyBranchId,
                    };
                    _context.VaccineSubscriptions.Add(subscriberModel);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public void CompanyFreeDays(Guid companyId)
        {
            try
            {
                var listOfCompanyModule = new List<CompanyModule>();
                if (companyId != Guid.Empty)
                {
                    var expiryDate = DateTime.Now.AddDays(30);
                    var companyModuleForDoc = new CompanyModule()
                    {
                        CompanyId = companyId,
                        SubcriptionStatus = CompanySubcriptionStatus.Active,
                        StartDate = DateTime.Now,
                        ModuleId = CompanySettings.DOCBookingModule,
                        ExpiryDate = expiryDate,
                    };
					listOfCompanyModule.Add(companyModuleForDoc);
					var companyModulesTransaction = new CompanyModule()
					{
						CompanyId = companyId,
						SubcriptionStatus = CompanySubcriptionStatus.Active,
						StartDate = DateTime.Now,
						ModuleId = CompanySettings.TransactionModule,
						ExpiryDate = expiryDate,
					};
					listOfCompanyModule.Add(companyModulesTransaction);
					var companyModulesVaccine = new CompanyModule()
					{
						CompanyId = companyId,
						SubcriptionStatus = CompanySubcriptionStatus.Active,
						StartDate = DateTime.Now,
						ModuleId = CompanySettings.VaccineModule,
						ExpiryDate = expiryDate,
					};
					listOfCompanyModule.Add(companyModulesTransaction);
					var companyModulesStore = new CompanyModule()
					{
						CompanyId = companyId,
						SubcriptionStatus = CompanySubcriptionStatus.Active,
						StartDate = DateTime.Now,
						ModuleId = CompanySettings.StoreModule,
						ExpiryDate = expiryDate,
					};
					listOfCompanyModule.Add(companyModulesTransaction);
				}
                _context.AddRange(listOfCompanyModule);
                _context.SaveChanges();
                UpdateCompanySettings(companyId);
			}
            catch (Exception ex)
            {
				string toEmail = _generalConfiguration.DeveloperEmail;
				string subject = "Exception On Giving Company Free Subscription";
				string message = ex.Message + " , <br /> This exception message occurred while trying to give company free package subscription ";
				_emailService.SendEmail(toEmail, subject, message);
				throw;
			}
        }
        public void UpdateCompanySettings(Guid companyId)
        {
            var modules = new CompanySetting()
            {
                CompanyId = companyId,
                DOCBookingModule = true,
                VaccineModule = true,
                StoreModule = true,
                TransactionModule = true,
            };
            _context.CompanySettings.Add(modules);
            _context.SaveChanges();
        }
    }
    public class drp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
