using Core.Db;
using Core.Models;
using Core.ViewModels;
using Logic.IHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace Vetman.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly ICustomerPaymentHelper _customerPaymentHelper;
        private readonly AppDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        private readonly IEmailHelper _emailHelper;

        public AccountController(AppDbContext context, ICustomerPaymentHelper customerPaymentHelper, IUserHelper userHelper, IAdminHelper adminHelper, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IEmailHelper emailHelper)
        {
            _signInManager = signInManager;
            _userHelper = userHelper;
            _userManager = userManager;
            _customerPaymentHelper = customerPaymentHelper;
            _context = context;
            _emailHelper = emailHelper;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Login(string email, string password)
        {
			if (email != null && password != null)
            {
                var user = await _userHelper.FindByEmailAsync(email).ConfigureAwait(false);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, password, true, true).ConfigureAwait(false);
					
					if (result.Succeeded)
                    {
                        var createdById = string.Empty;
                        if (user.CompanyBranch?.Company != null)
                        {
                            createdById = user.CompanyBranch.Company.CreatedById;
                            user.CompanyBranch.Company.CreatedBy = null;
                        }                           
                        user.Roles = (List<string>)await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                        foreach (var userRole in user.Roles)
                        {
                            if (userRole == Utitily.Constants.SuperAdminRole)
                            {
                                user.UserRole = Utitily.Constants.SuperAdminRole;
                            }
                            else if (userRole == Utitily.Constants.CompanyAdminRole)
                            {
                                user.UserRole = Utitily.Constants.CompanyAdminRole;
                            }
                            else
                            {
                                user.UserRole = Utitily.Constants.CompanyStaffRole;
                            }
                        }
                        var httpContext = new DefaultHttpContext();
                        var baseUrl = $"{this.Request.Host}{this.Request.PathBase}";
                        user.BaseUrl = baseUrl;
                        var currentUser = JsonConvert.SerializeObject(user);
                        HttpContext.Session.SetString("myuser", currentUser);
                        var roleId = _context.UserRoles.Where(x => x.UserId == user.Id).FirstOrDefault()?.RoleId;
                        var screenRoles = _context.ScreenRoles.Where(x => x.CompanyBranchId == user.CompanyBranchId && x.RoleId == roleId).ToList();
                        if (user.CompanyBranch != null)
                        {
                            if (user.CompanyBranch.CompanyId != Guid.Empty)
                            {
                                var vaccineSubscription = _context.CompanySettings.Where(x => x.CompanyId == user.CompanyBranch.CompanyId)?.FirstOrDefault();
                                if (vaccineSubscription != null)
                                {
                                    var companySub = JsonConvert.SerializeObject(vaccineSubscription);
                                    HttpContext.Session.SetString("vaccineSubscription", companySub);
                                }

                            }
                        }

                        if (screenRoles.Count != 0)
                        {
                            var accessScreens = JsonConvert.SerializeObject(screenRoles);
                            HttpContext.Session.SetString("accessRight", accessScreens);
                        }
                        var url = _userHelper.GetValidatedUrl();
                        var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                        if (ip == "::1")
                            ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();
                        var getIp = new string[] { ip.ToString() };
                        var systemName = Request.Headers["User-Agent"].ToString();

                        if (createdById != string.Empty)
                        {
                            user.CompanyBranch.Company.CreatedBy = user;
                            user.CompanyBranch.Company.CreatedById = user.Id;
                        }
                       _userHelper.CreateUserLoginLog(ip, user.Id, systemName);

                        return Json(new { isError = false, dashboard = url });
                    }
                }
                return Json(new { isError = true, msg = "Invalid Password Or Username" });
            }
            return Json(new { isError = true, msg = "Password and Confirm password required" });
        }

        [HttpPost]
        public async Task<JsonResult> ValidateEmail(string email)
        {
            if (email != null)
            {
                var user = await _userHelper.FindByEmailAsync(email).ConfigureAwait(false);
                if (user != null)
                {
                    return Json(new { isError = false });
                }
                return Json(new { isError = true });
            }
            return Json(new { isError = true });
        }

        [HttpGet]
        public IActionResult CompanyRegistration()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> CompanyRegistration(string companyDetails)
        {
            if (companyDetails != null)
            {
                var companyViewModel = JsonConvert.DeserializeObject<CompanyViewModel>(companyDetails);
                if (companyViewModel != null)
                {
                    var checkForCompanyName = _context.Companies.Where(x => x.Name == companyViewModel.Name).FirstOrDefault();
                    if (checkForCompanyName != null)
                    {
                        return Json(new { isError = true, msg = "Company Name belongs to another Company" });
                    }
                    var checkForEmail = await _userHelper.FindByEmailAsync(companyViewModel.Email).ConfigureAwait(false);
                    if (checkForEmail != null)
                    {
                        return Json(new { isError = true, msg = "Email belongs to another Company" });
                    }
                    if (companyViewModel.Password != companyViewModel.ConfirmPassword)
                    {
                        return Json(new { isError = true, msg = "Password and Confirm password must match" });
                    }
                    var createCompany = await _customerPaymentHelper.CreateCompany(companyViewModel).ConfigureAwait(false);
                    if (createCompany)
                    {
                        return Json(new { isError = false, msg = "Company registered successfully, login to continue" });
                    }
                    return Json(new { isError = true, msg = "Unable to create Company" });
                }
            }
            return Json(new { isError = true, msg = "Error Occurred" });
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ResetPassword(Guid token)
        {
            if (token != Guid.Empty)
            {
                var passwordResetViewmodel = new PasswordResetViewmodel()
                {
                    Token = token,
                };
                return View(passwordResetViewmodel);
            }
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ResetPassword(string passwordResetViewmodel)
        {
            var userdetail = JsonConvert.DeserializeObject<PasswordResetViewmodel>(passwordResetViewmodel);
            if (userdetail != null)
            {
                if (userdetail.Password != userdetail.ConfirmPassword)
                {
                    return Json(new { isError = true, msg = "Password and confirm password must match" });
                }
                var userVerification = await _userHelper.GetUserToken(userdetail.Token).ConfigureAwait(false);
                if (userVerification != null && !userVerification.Used)
                {
                    await _userManager.RemovePasswordAsync(userVerification.User).ConfigureAwait(false);
                    await _userManager.AddPasswordAsync(userVerification.User, userdetail.Password).ConfigureAwait(false);
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    await _userHelper.MarkTokenAsUsed(userVerification).ConfigureAwait(false);

                    var sendEmail = _emailHelper.PasswordResetConfirmation(userVerification.User);
                    if (sendEmail)
                    {
                        return Json(new { isError = false, msg = "Your Password has been reset successfully, you can now use the new password on your next login" });
                    }
                    return Json(new { isError = true, mgs = "Sorry! The Link You Entered is Invalid or Expired " });
                }
            }
            return Json(new { isError = true, mgs = "Error occurred" });
        }
      
    }
}
