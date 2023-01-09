using Core.Db;
using Core.Models;
using Core.ViewModels;
using Hangfire;
using Logic.IHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using static Core.Enums.VetManEnums;

namespace Logic.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly AppDbContext _context;
        private readonly AppDbContext _adminHelper;
        private readonly UserManager<ApplicationUser> _userManager;
        private IHttpContextAccessor _httpContext;
        private readonly ICompanyHelper _companyHelper;

        public UserHelper(AppDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, ICompanyHelper companyHelper, AppDbContext adminHelper)
        {
            _context = context;
            _userManager = userManager;
            _httpContext = httpContextAccessor;
            _companyHelper = companyHelper;
            _adminHelper = adminHelper;
        }

        public async Task<ApplicationUser> FindByUserNameAsync(string username)
        {
            return await _userManager.Users.Where(s => s.UserName == username)?.FirstOrDefaultAsync();
        }

        public async Task<ApplicationUser> CreateCompanyAdminUser(CompanyViewModel userDetails)
        {
            var user = new ApplicationUser();
            user.UserName = userDetails.Email;
            user.Email = userDetails.Email;
            user.FirstName = userDetails.FirstName;
            user.LastName = userDetails.LastName;
            user.PhoneNumber = userDetails.Phone;
            user.Address = userDetails.Address;
            user.DateRegistered = DateTime.Now;
            user.IsDeactivated = false;
            // user.IsAdmin = userDetails.IsAdmin;
            var createUser = await _userManager.CreateAsync(user, userDetails.Password).ConfigureAwait(false);
            if (createUser.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "CompanyAdmin").ConfigureAwait(false);
                return user;
            }
            return null;
        }
        public async Task<ApplicationUser>? FindByEmailAsync(string email)
        {
            try
            {
                var user = await _context.ApplicationUsers.Where(s => s.Email == email).Include(x => x.CompanyBranch).Include(x => x.CompanyBranch.Company).FirstOrDefaultAsync().ConfigureAwait(false);
                if (user != null)
                {
                    return user;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return null;
        }

        public string GetRoleLayout()
        {
            var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = UpdateSessionAsync(loggedInUser.UserName).Result;

			}
			if (loggedInUser != null)
            {
                var superAdmin = loggedInUser.Roles.Contains(Utitily.Constants.SuperAdminRole);
                if (superAdmin)
                {
                    return Utitily.Constants.SuperAdminLayout;
                }
                else if (!superAdmin)
                {
                    var isCompanyAdmin = loggedInUser.Roles.Contains(Utitily.Constants.CompanyAdminRole);
                    if (isCompanyAdmin)
                    {
                        return Utitily.Constants.CompanyAdminLayout;
                    }
                    else
                    {
                        var isBrachManager = loggedInUser.Roles.Contains(Utitily.Constants.CompanyManagerRole);
                        if (isBrachManager)
                        {
                            return Utitily.Constants.CompanyMangerLayout;
                        }
                        else
                        {
                            return Utitily.Constants.CompanyStaffLayout;
                        }
                    }
                }
            }
            return Utitily.Constants.DefaultLayout;
        }
        public List<ApplicationUserViewModel> GetStaffList()
        {
            var applicationUserViewModel = new List<ApplicationUserViewModel>();
            var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = UpdateSessionAsync(loggedInUser.UserName)?.Result;
			}
			var company = _context.CompanyBranches.Where(x => x.Id == loggedInUser.CompanyBranchId).Include(x => x.Company).FirstOrDefault();
            if (company != null)
            {
                    var allStaff = _context.ApplicationUsers.Where(x => !x.IsDeactivated && x.CompanyBranchId == loggedInUser.CompanyBranchId)
                       .Select(s => new ApplicationUserViewModel()
                       {
                           Id = s.Id,
                           FirstName = s.FirstName,
                           LastName = s.LastName,
                           CompanyBranch = s.CompanyBranch.Name,
                           Email = s.Email,
                           DateRegistered = s.DateRegistered.ToString("d"),
                           PhoneNumber = s.PhoneNumber,
                           Address = s.Address,
                       }).ToList();
                    if (allStaff != null && allStaff.Count > 0)
                    {
                        return allStaff;
                    }
                
            }
            return applicationUserViewModel;
        }
        public async Task<ApplicationUser> CreateStaffDetails(ApplicationUserViewModel applicationUserView)
        {
            var user = new ApplicationUser();
            user.UserName = applicationUserView.Email;
            user.Email = applicationUserView.Email;
            user.FirstName = applicationUserView.FirstName;
            user.LastName = applicationUserView.LastName;
            user.DateRegistered = DateTime.Now;
            user.IsDeactivated = false;
            user.CompanyBranchId = applicationUserView.CompanyBranchId;
            var createUser = await _userManager.CreateAsync(user, applicationUserView.Password).ConfigureAwait(false);
            if (createUser.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "CompanyStaff").ConfigureAwait(false);
                return user;
            }
            return null;
        }

        public ApplicationUser EditStaffDetails(string staffId)
        {
            if (staffId != null)
            {
                var staffToEdit = _userManager.Users.Where(x => x.Id == staffId && !x.IsDeactivated).FirstOrDefault();
                if (staffToEdit != null)
                    return staffToEdit;
            }
            return null;
        }
        public ApplicationUser EditStaffDetails(ApplicationUserViewModel ApplicationUserDetails)
        {
            if (ApplicationUserDetails != null)
            {
                var staffDetails = _userManager.Users.Where(x => x.Id == ApplicationUserDetails.Id && !x.IsDeactivated).FirstOrDefault();
                if (staffDetails != null)
                {
                    staffDetails.FirstName = ApplicationUserDetails.FirstName;
                    staffDetails.LastName = ApplicationUserDetails.LastName;
                    staffDetails.CompanyBranchId = ApplicationUserDetails.CompanyBranchId;

                    _context.Update(staffDetails);
                    _context.SaveChanges();
                    return staffDetails;
                }
            }
            return null;
        }
        public bool DeleteStaffDetails(ApplicationUserViewModel ApplicationUserDetails)
        {
            if (ApplicationUserDetails != null)
            {
                var staffDetails = _userManager.Users.Where(x => x.Id == ApplicationUserDetails.Id && !x.IsDeactivated).FirstOrDefault();
                if (staffDetails != null)
                {
                    staffDetails.IsDeactivated = true;

                    _context.Update(staffDetails);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public string GetValidatedUrl()
        {
            var loggedInUser = Utitily.GetCurrentUser();
            if (loggedInUser.Id == null)
            {
                loggedInUser = UpdateSessionAsync(loggedInUser.UserName)?.Result;
			}
            var superAdmin = loggedInUser.Roles.Contains(Utitily.Constants.SuperAdminRole);
            if (superAdmin)
            {
                return "/SuperAdmin/Index";
            }
            var isCompanyAdmin = loggedInUser.Roles.Contains(Utitily.Constants.CompanyAdminRole);
            if (isCompanyAdmin)
            {
                return "/Admin/Index";
            }
            var isCompanyManager = loggedInUser.Roles.Contains(Utitily.Constants.CompanyManagerRole);
            if (isCompanyManager)
            {
                return "/Branches/Index";
            }
            var isCompanyStaff = loggedInUser.Roles.Contains(Utitily.Constants.CompanyStaffRole);
            if (isCompanyStaff)
            {
                return "/Staff/Index";
            }
            return "/Account/Login";
        }

        public async Task<ImpersonationViewModel> CheckForImpersonation(ApplicationUser loggedInUser)
        {
            var session = _httpContext.HttpContext.Session;
            if (loggedInUser != null)
            {
                if (loggedInUser.UserRole == Utitily.Constants.SuperAdminRole)
                {
                    return null;
                }
                var impersonation = new ImpersonationViewModel();
                var impersonatee = await _companyHelper.GetCompanyByAdminId(loggedInUser.Id).ConfigureAwait(false);
                if (impersonatee != null)
                {
                    var impersonationRecord = GetLatestImpersonateeRecord(loggedInUser.Id).Result;
                    if (impersonationRecord != null)
                    {
                        var SessionKeyName = impersonationRecord.CompanyAdminId;
                        var endSession = session.GetString(SessionKeyName);
                        impersonation.ImpersonationRecord = impersonationRecord;
                        impersonation.Impersonator = FindById(impersonationRecord.SuperAdminUserId);
                        impersonation.AdminBeingImpersonated = loggedInUser;
                        impersonation.IsImpersonatorAdmin = CheckIfUserIsSuperAdmin(impersonationRecord.SuperAdminUserId).Result;
                        impersonation.Impersonatee = impersonatee;
                        impersonation.ShowEndSession = endSession;
                        return impersonation;
                    }
                }
            }
            return null;
        }
        public async Task<Impersonation> GetLatestImpersonateeRecord(string adminId)
        {
            if (adminId != null)
            {
                var lastTimeImpersonated = _context.Impersonations.Where(x => x.CompanyAdminId == adminId).OrderBy(x => x.DateImpersonated)?.LastOrDefault();
                if (lastTimeImpersonated != null)
                {
                    return lastTimeImpersonated;
                }
            }
            return null;
        }
        public ApplicationUser FindById(string Id)
        {
            return _context.ApplicationUsers.Where(s => s.Id == Id)?.Include(s => s.CompanyBranch).Include(s => s.CompanyBranch.Company).FirstOrDefault();
        }
        public async Task<bool> CheckIfUserIsSuperAdmin(string userId)
        {
            if (userId == null)
            {
                return false;
            }
            var currentUser = FindById(userId);
            if (currentUser != null)
            {
                var goAdmin = await _userManager.IsInRoleAsync(currentUser, "SuperAdmin");

                return goAdmin;
            }
            return false;
        }
        public async Task<UserVerification> GetUserToken(Guid token)
        {
            return await _context.UserVerifications.Where(t => t.Used != true && t.Token == token)?.Include(s => s.User).FirstOrDefaultAsync();
        }
        public async Task<bool> MarkTokenAsUsed(UserVerification userVerification)
        {
            var VerifiedUser = _context.UserVerifications.Where(s => s.UserId == userVerification.User.Id && s.Used != true).FirstOrDefault();
            if (VerifiedUser != null)
            {
                userVerification.Used = true;
                userVerification.DateUsed = DateTime.Now;
                _context.Update(userVerification);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public List<SupplierViewModel> GetSupplierList()
        {
            var supplierViewModel = new List<SupplierViewModel>();
            var loggedInUser = Utitily.GetCurrentUser();
            if (loggedInUser.Id == null)
            {
                loggedInUser = UpdateSessionAsync(loggedInUser.UserName)?.Result;
            }
            if (loggedInUser != null)
            {
                var allSupplier = _context.Suppliers.Where(x => !x.Deleted && x.CompanyBranchId == loggedInUser.CompanyBranchId)
               .Select(s => new SupplierViewModel()
               {
                   Id = s.Id,
                   Name = s.Name,
                   Address = s.Address,
                   CompanyName = s.CompanyName,
                   Email = s.Email,
                   PhoneNumber = s.PhoneNumber,
               }).ToList();
                if (allSupplier != null && allSupplier.Count > 0)
                {
                    return allSupplier;
                }
            }
            return supplierViewModel;
        }

        public bool CreateSupplierDetails(SupplierViewModel supplierDetails)
        {
            var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = UpdateSessionAsync(loggedInUser.UserName).Result;

			}
			if (loggedInUser != null)
            {
                var detailsValidation = _context.Suppliers.Where(c => c.PhoneNumber == supplierDetails.PhoneNumber && c.CompanyBranchId == loggedInUser.CompanyBranchId).FirstOrDefault();
                if (detailsValidation == null)
                {
                    var suppliersDoc = new Supplier()
                    {
                        Name = supplierDetails.Name,
                        DateCreated = DateTime.Now,
                        Address = supplierDetails.Address,
                        PhoneNumber = supplierDetails.PhoneNumber,
                        Email = supplierDetails.Email,
                        CompanyName = supplierDetails.CompanyName,
                        Active = true,
                        Deleted = false,
                        CompanyBranchId = loggedInUser.CompanyBranchId,
                    };
                    _context.Suppliers.Add(suppliersDoc);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        public bool CheckSupplierPhoneNumber(SupplierViewModel supplierDetails)
        {
            var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = UpdateSessionAsync(loggedInUser.UserName).Result;

			}
			if (loggedInUser != null)
            {
                var detailsValidation = _context.Suppliers.Where(c => c.PhoneNumber == supplierDetails.PhoneNumber && c.CompanyBranchId == loggedInUser.CompanyBranchId).FirstOrDefault();
                if (detailsValidation == null)
                {
                    return true;
                }
            }
            return false;
        }
        public Supplier EditSupplier(int id)
        {
            var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = UpdateSessionAsync(loggedInUser.UserName).Result;

			}
			if (loggedInUser != null)
            {
                if (id != 0)
                {
                    var Supplier2Edit = _context.Suppliers.Where(x => x.Id == id && x.Active && !x.Deleted && x.CompanyBranchId == loggedInUser.CompanyBranchId).FirstOrDefault();
                    if (Supplier2Edit != null)
                        return Supplier2Edit;
                }
            }
            return null;
        }
        public Supplier SupplierEditedDetails(SupplierViewModel supplierDetails)
        {
            var loggedInuser = Utitily.GetCurrentUser();
			if (loggedInuser.Id == null)
			{
				loggedInuser = UpdateSessionAsync(loggedInuser.UserName).Result;

			}
			if (loggedInuser != null)
            {
                if (supplierDetails != null)
                {
                    var SupoDetails = _context.Suppliers.Where(x => x.Id == supplierDetails.Id && x.Active && !x.Deleted).FirstOrDefault();
                    if (SupoDetails != null)
                    {
                        SupoDetails.Name = supplierDetails.Name;
                        SupoDetails.Address = supplierDetails.Address;
                        SupoDetails.PhoneNumber = supplierDetails.PhoneNumber;
                        SupoDetails.CompanyName = supplierDetails.CompanyName;
                        SupoDetails.Email = supplierDetails.Email;
                        _context.Update(SupoDetails);
                        _context.SaveChanges();
                        return SupoDetails;
                    }
                }
            }
            return null;
        }
        public bool DeleteSupplierDetails(SupplierViewModel supplierDetails)
        {
            if (supplierDetails != null)
            {
                var supdetails = _context.Suppliers.Where(x => x.Id == supplierDetails.Id && x.Active && !x.Deleted).FirstOrDefault();
                if (supdetails != null)
                {
                    supdetails.Deleted = true;
                    supdetails.Active = false;
                    _context.Update(supdetails);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }
		public async Task<ApplicationUser>? UpdateSessionAsync(string userEmail)
		{
			try
			{
				var user = await _context.ApplicationUsers.Where(s => s.Email == userEmail && s.UserName == userEmail).Include(x => x.CompanyBranch).Include(x => x.CompanyBranch.Company).FirstOrDefaultAsync().ConfigureAwait(false);
				if (user != null)
				{
					if (user.CompanyBranch?.Company != null)
						user.CompanyBranch.Company.CreatedBy = null;
					var currentUser = JsonConvert.SerializeObject(user);
                	AppHttpContext.Current.Session.SetString("myuser", currentUser);
                    var loggedInUser = Utitily.GetCurrentUser();
                    return loggedInUser;
				}
			}
			catch (Exception e)
			{
				throw e;
			}
			return null;
		}

        public void CallHangFire(string ipAddress, string userId, string systemName)
        {
            BackgroundJob.Enqueue(() => CreateUserLoginLog(ipAddress, userId, systemName));
        }

        public void CreateUserLoginLog(string ipAddress,string userId, string systemName)
        {
            try
            {
                if (userId != null)
                {
                    ////For IPAdress.
                    //IPHostEntry iphostinfo = Dns.GetHostEntry(Dns.GetHostName());
                    //string IpAddress = Convert.ToString(iphostinfo.AddressList.FirstOrDefault(address => address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork));

                    // For DeviceName.
                    //string MachineName = Environment.MachineName;
                  
                    var userDetails = new UserLoginLog()
                    {
                        DeviceName = systemName,
                        IPAddress = ipAddress,
                        LoginDate = DateTime.Now,
                        UserId = userId,
                    };
                    _context.UserLoginLogs.Add(userDetails);
                    _context.SaveChanges();
                }
            }
            catch (Exception exp)
            {

                throw exp;
            }
        }
             
        public List<UserLogsViewModel> GetLogInUserList()
        {
             var getListOfUsers = _context.UserLoginLogs.Where(r => r.UserId != null && r.IPAddress != null)
            .Include(x => x.User)
            .Select(c => new UserLogsViewModel
            {
                IpAddress = c.IPAddress,
                UserName = c.User.FullName,
                DeviceName = c.DeviceName,
                LoggedDate = c.LoginDate,
            }).ToList();
               
            return getListOfUsers;
        }

        public CompanyModule ModulesInfo()
        {
            var loggedInUser = Utitily.GetCurrentUser();
            if (loggedInUser.Id == null)
            {
                loggedInUser = UpdateSessionAsync(loggedInUser.UserName).Result;
            }
            if (loggedInUser != null)
            {
                //var getCompanyId = _context.CompanyBranches.Where(x => x.Id == loggedInUser.CompanyBranchId && !x.Deleted).FirstOrDefault();
                //if (getCompanyId.CompanyId != Guid.Empty)
                //{
                    var getCompanySubEndDate = _context.CompanyModules.Where(c => c.CompanyId == loggedInUser.CompanyBranch.CompanyId).FirstOrDefault();
                    if(getCompanySubEndDate != null)
                        return getCompanySubEndDate;
                //}
            }
            return null;
        }
        public ApplicationUserViewModel ModulesInfoForStaff()
		{
            
            var loggedInUser = Utitily.GetCurrentUser();
            if (loggedInUser.Id == null)
            {
                loggedInUser = UpdateSessionAsync(loggedInUser.UserName).Result;
            }
            if (loggedInUser != null)
            {
                var getCompanyId = _context.CompanyBranches.Where(x => x.Id == loggedInUser.CompanyBranchId && !x.Deleted).FirstOrDefault();
                if (getCompanyId.CompanyId != Guid.Empty)
                {
                    var getCompanySubEndDate = _context.CompanyModules.Where(c => c.CompanyId == getCompanyId.CompanyId && c.Id != Guid.Empty).FirstOrDefault();
                    if (getCompanySubEndDate != null)
					{
                        var staffDetails = new ApplicationUserViewModel()
                        {
                            ExpiryDate = getCompanySubEndDate.ExpiryDate,
                            CompanyId = getCompanySubEndDate.CompanyId,
                            ModuleId = getCompanySubEndDate.ModuleId,
                            SubcriptionStatus = getCompanySubEndDate.SubcriptionStatus,
                        };
                        return staffDetails;
                    }
                }
            }
            return null;
        }
        
        public List<PaymentMeans> GetPayementMeans(int id)
        {
            var loggedInUser = Utitily.GetCurrentUser();
            if (loggedInUser.Id == null)
            {
                loggedInUser = UpdateSessionAsync(loggedInUser.UserName).Result;

            }
            var common = new PaymentMeans()
            {
                Id = 0,
                Name = "-- Select --"
            };
            var getpaymentMeans = _context.PaymentMeans.Where(x => x.CommonDropDownId == id && !x.Deleted && x.CompanyBranchId == loggedInUser.CompanyBranchId ).ToList();
            getpaymentMeans.Insert(0, common);
            return getpaymentMeans;
        } 
        public List<CommonDropDown> GetBank()
        {
            var loggedInUser = Utitily.GetCurrentUser();
            if (loggedInUser.Id == null)
            {
                loggedInUser = UpdateSessionAsync(loggedInUser.UserName).Result;

            }
            var common = new CommonDropDown()
            {
                Id = 0,
                Name = "-- Select --"
            };
            var getbank = _context.CommonDropDowns.Where(x => !x.Deleted && x.CompanyBranchId == loggedInUser.CompanyBranchId).ToList();
            getbank.Insert(0, common);
            return getbank;
        }
        public bool CreateBankDetails(CommonDropDowns bankDetails)
        {
            if (bankDetails != null)
            {
                var loggedInUser = Utitily.GetCurrentUser();
                if (loggedInUser.Id == null)
                {
                    loggedInUser = UpdateSessionAsync(loggedInUser.UserName).Result;

                }
                if (loggedInUser != null)
                {
                    var detailsValidation = _context.CommonDropDowns.Where(c => !c.Deleted && c.CompanyBranchId == loggedInUser.CompanyBranchId && c.Name == bankDetails.Name).FirstOrDefault();
                    if (detailsValidation == null)
                    {
                        var bankData = new CommonDropDown()
                        {
                            Name = bankDetails.Name,
                            DateCreated = DateTime.Now,
                            Active = true,
                            Deleted = false,
                            CompanyBranchId = loggedInUser.CompanyBranchId,
                        };
                        _context.CommonDropDowns.Add(bankData);
                        _context.SaveChanges();
                        return true;
                    }
                }
            }
            return false;
        }
        public List<CommonDropDown> GetActiveBanks()
        {
            var loggedInUser = Utitily.GetCurrentUser();
            if (loggedInUser.Id == null)
            {
                loggedInUser = UpdateSessionAsync(loggedInUser.UserName).Result;

            }
            var getActivebank = _context.CommonDropDowns.Where(x => !x.Deleted && x.CompanyBranchId == loggedInUser.CompanyBranchId).ToList();
            return getActivebank;
        }
        
        public CommonDropDown DeleteBankDetails(CommonDropDowns? bankData)
        {
            if (bankData != null)
            {
                var bank = _context.CommonDropDowns.Where(x => x.Id == bankData.Id && x.Active && !x.Deleted).FirstOrDefault();
                if (bank != null)
                {
                    bank.Active = false;
                    bank.Deleted = true;

                    _context.Update(bank);
                    _context.SaveChanges();
                    return bank;
                }
            }
            return null;
        }
        public bool CreateBankPaymentMeansDetails(PaymentMeansViewModel paymentMeansViewModel)
        {
            if (paymentMeansViewModel != null)
            {
                var loggedInUser = Utitily.GetCurrentUser();
                if (loggedInUser.Id == null)
                {
                    loggedInUser = UpdateSessionAsync(loggedInUser.UserName).Result;

                }
                if (loggedInUser != null)
                {
                    var detailsValidation = _context.PaymentMeans.Where(c => !c.Deleted && c.CompanyBranchId == loggedInUser.CompanyBranchId && c.Name == paymentMeansViewModel.Name).FirstOrDefault();
                    if (detailsValidation == null)
                    {
                        var PaymentData = new PaymentMeans()
                        {
                            Name = paymentMeansViewModel.Name,
                            DateCreated = DateTime.Now,
                            CommonDropDownId = paymentMeansViewModel.CategoryId,
                            Active = true,
                            Deleted = false,
                            CompanyBranchId = loggedInUser.CompanyBranchId,
                        };
                        _context.PaymentMeans.Add(PaymentData);
                        _context.SaveChanges();
                        return true;
                    }
                }
            }
            return false;
        }
        public bool CreatePaymentDetails(SalesPaymentViewModel paymentDetails)
		{
            try
            {
                var loggedInUser = Utitily.GetCurrentUser();
                if (loggedInUser.Id == null)
                    loggedInUser = UpdateSessionAsync(loggedInUser.UserName)?.Result;
                var productModel = new SalesPayment();
                
                if (loggedInUser != null)
                {
                    if (paymentDetails.PaidCash == true)
                    {
                        var getcash = _context.PaymentMeans.Where(f => f.Name == "Cash" && !f.Deleted).FirstOrDefault();
                        productModel = new SalesPayment()
                        {
                            AmountPaid = paymentDetails.AmountPaid,
                            Balance = paymentDetails.Balance,
                            OrderId = paymentDetails.OrderId,
                            SalesLogsId = paymentDetails.SalesLogsId,
                            PaymentMeansId = getcash.Id,
                            CompanyBranchId = loggedInUser.CompanyBranchId,
                            DepositType = DropdownEnums.CashDeposit,
                            PaymentDate = paymentDetails.DateCreated,
                        };
                    }
                    else
                    {
                        productModel = new SalesPayment()
                        {
                            AmountPaid = paymentDetails.AmountPaid,
                            Balance = paymentDetails.Balance,
                            OrderId = paymentDetails.OrderId,
                            SalesLogsId = paymentDetails.SalesLogsId,
                            CompanyBranchId = loggedInUser.CompanyBranchId,
                            PaymentMeansId = paymentDetails.SelectPayment,
                            DepositType = DropdownEnums.BankDeposit,
                            PaymentDate = paymentDetails.DateCreated,
                        };
                    }
                    _context.SalesPayments.Add(productModel);
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
    }

}