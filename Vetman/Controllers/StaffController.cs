using Core.Db;
using Core.Models;
using Core.ViewModels;
using Logic.IHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Logic.AppHttpContext;

namespace Vetman.Controllers
{
	[SessionTimeout]
	public class StaffController : Controller
	{

		private AppDbContext _context;
		private IAdminHelper _adminHelper;
		private IUserHelper _userHelper;
		private readonly IEmailHelper _emailHelper;
		private readonly IDropdownHelper _dropdownHelper;
		private readonly UserManager<ApplicationUser> _userManager;

		public StaffController(AppDbContext context, IUserHelper userHelper, IAdminHelper adminHelper, IEmailHelper emailHelper, IDropdownHelper dropdownHelper, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_adminHelper = adminHelper;
			_userHelper = userHelper;
			_emailHelper = emailHelper;
			_dropdownHelper = dropdownHelper;
			_userManager = userManager;
		}

        [HttpGet]
        public IActionResult Index()
        {
            var utility = new Utitily();
            ViewBag.Layout = utility.GetRoleLayout();
            var modulesInfo = _userHelper.ModulesInfoForStaff();
            if(modulesInfo != null)
                return View(modulesInfo);
            return View();
        }


		[HttpPost]
		public async Task<JsonResult> CreateStaff(string staffDetails)
		{
			if (staffDetails != null)
			{
				var applicationUser = JsonConvert.DeserializeObject<ApplicationUserViewModel>(staffDetails);
				if (applicationUser != null)
				{
					var checkForEmail = await _userHelper.FindByEmailAsync(applicationUser.Email).ConfigureAwait(false);
					if (checkForEmail != null)
					{
						return Json(new { isError = true, msg = "Email Belongs To Another Staff" });
					}
					if (applicationUser.Password != applicationUser.ConfirmPassword)
					{
						return Json(new { isError = true, msg = "Password and Confirm password must match" });
					}
					if (applicationUser.CompanyBranchId == Guid.Empty)
					{
						return Json(new { isError = true, msg = "Select Company Branch" });
					}
					var createUser = await _userHelper.CreateStaffDetails(applicationUser).ConfigureAwait(false);
					if (createUser != null)
					{
						var userToken = await _emailHelper.CreateUserToken(createUser.Email).ConfigureAwait(false);
						if (userToken != null)
						{
							string linkToClick = HttpContext.Request.Scheme.ToString() + "://" + HttpContext.Request.Host.ToString()
								+ "/Account/ResetPassword?token=" + userToken.Token;
							var sendEmail = _emailHelper.PasswordResetLink(createUser, linkToClick);
							if (sendEmail)
							{
								return Json(new { isError = false, msg = "Staff Created Successfully. A password reset link have been sent to staff email" });
							}
							return Json(new { isError = false, msg = "Staff Created Successfully and unable to send password reset link" });
						}
					}
				}
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}
		[HttpGet]
		public JsonResult EditedStaff(string staffId)
		{
			try
			{
				if (staffId != null)
				{
					var staffToEdit = _userHelper.EditStaffDetails(staffId);
					if (staffToEdit != null)
					{
						return Json(new { isError = false, data = staffToEdit });
					}
				}
				return Json(new { isError = true, msg = "Could not Edit Staff." });
			}
			catch (Exception exp)
			{

				throw exp;
			}
		}
		[HttpPost]
		public JsonResult EditedStaffs(string staff)
		{
			try
			{
				var staffData = JsonConvert.DeserializeObject<ApplicationUserViewModel>(staff);
				if (staffData != null)
				{
					var updateProductDetails = _userHelper.EditStaffDetails(staffData);
					if (updateProductDetails != null)
					{
						return Json(new { isError = false, msg = "Staff Edited Successfully." });
					}
				}
				return Json(new { isError = true, msg = "Something went wrong." });
			}
			catch (Exception exp)
			{
				throw exp;
			}
		}
		[HttpPost]
		public JsonResult DeleteStaff(string staffDetails)
		{
			try
			{
				if (staffDetails != null)
				{
					var staffData = JsonConvert.DeserializeObject<ApplicationUserViewModel>(staffDetails);
					if (staffData != null)
					{
						var staff = _userManager.FindByIdAsync(staffData.Id).Result;
						var userRole = _userManager.GetRolesAsync(staff).Result.FirstOrDefault();
						if (userRole != Utitily.Constants.CompanyAdminRole)
						{
							var updateStaffDetails = _userHelper.DeleteStaffDetails(staffData);
							if (updateStaffDetails)
							{
								return Json(new { isError = false, msg = "Staff Deleted Successfully." });
							}
						}
						else
						{
							return Json(new { isError = true, msg = "Oops!, Sorry you can't delete addmin account." });
						}
					}
				}
				return Json(new { isError = true, msg = "Something went wrong." });
			}
			catch (Exception exp)
			{
				throw exp;
			}
		}

		[HttpGet]
		public IActionResult CompanyStaff()
		{
			try
			{
				var loggedInUser = Utitily.GetCurrentUser();
				if (loggedInUser.Id == null)
				{
					loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

				}
				if (loggedInUser.UserRole != Utitily.Constants.CompanyStaffRole)
				{
					ViewBag.Layout = _userHelper.GetRoleLayout();
					ViewBag.CompanyBranches = _dropdownHelper.GetAllCompanyBranch();
					var staffsList = _userHelper.GetStaffList();
					if (staffsList != null && staffsList.Count > 0)
					{
						return View(staffsList);
					}
					return View(staffsList);
				}
				return RedirectToAction("CantAccess", "Admin");
			}
			catch (Exception)
			{

				throw;
			}

		}
	}
}
