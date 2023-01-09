using Core.Db;
using Core.Models;
using Core.ViewModels;
using Logic.IHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static Logic.AppHttpContext;

namespace Vetman.Controllers
{
	[SessionTimeout]
	[Authorize(Roles = "SuperAdmin")]
	public class SuperAdminController : Controller
	{
		private readonly IAdminHelper _adminHelper;
		private readonly ISuperAdminHelper _superAdminHelper;
		private readonly IUserHelper _userHelper;
		private readonly ICompanyHelper _companyHelper;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly AppDbContext _context;
		private readonly ICustomerPaymentHelper _customerPaymentHelper;
		private readonly IDropdownHelper _dropdownHelper;
		private readonly UserManager<ApplicationUser> _userManager;

		public SuperAdminController(AppDbContext context, IUserHelper userHelper, IAdminHelper adminHelper, ICustomerPaymentHelper customerPaymentHelper, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ICompanyHelper companyHelper, IDropdownHelper dropdownHelper, ISuperAdminHelper superAdminHelper)
		{
			_context = context;

			_userHelper = userHelper;
			_adminHelper = adminHelper;
			_customerPaymentHelper = customerPaymentHelper;
			_signInManager = signInManager;
			_userManager = userManager;
			_companyHelper = companyHelper;
			_dropdownHelper = dropdownHelper;
			_superAdminHelper = superAdminHelper;
		}

		[HttpGet]
		public IActionResult Index()
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
			var allCompanies = _customerPaymentHelper.GetAllCompanies();
			if (allCompanies != null && allCompanies.Count > 0)
			{
				return View(allCompanies);
			}
			return View(allCompanies);
		}

		[HttpGet]
		public async Task<IActionResult> CompanySetting(Guid companyId)
		{
			var companySettings = await _companyHelper.GetCompanySettings(companyId).ConfigureAwait(false);
			if (companySettings != null)
			{
				return View(companySettings);
			}
			return View(companySettings);
		}

		[HttpPost]
		public JsonResult CompanyCustomSetting(Guid companyId, List<string> checkedCompanySettings, List<string> uncheckedCompanySettings)
		{
			var companySetting = _companyHelper.GetCompanyCustomSettings(companyId, checkedCompanySettings, uncheckedCompanySettings);
			if (companySetting != null)
			{
				return Json(new { isError = false, msg = "Company settings updated successfully", });
			}
			return Json(new { isError = true, msg = "Could not update company settings", data = companySetting });
		}
		public async Task<JsonResult> ImpersonateCompanyAdmin(string email)
		{
			if (email != null)
			{
				var session = HttpContext.Session;
				var superAdmin = Utitily.GetCurrentUser();
				if (superAdmin.Id == null)
				{
					superAdmin = _userHelper.UpdateSessionAsync(superAdmin.UserName).Result;

				}
				var getCompanyAdmin = _userManager.Users.Where(x => x.Email == email)
					.Include(c => c.CompanyBranch).Include(f => f.CompanyBranch.Company).FirstOrDefault();
				//var getCompany = _context.Companies.Where(x => x.Email.ToLower().Trim() == email.ToLower().Trim()).Include(x => x.CreatedBy).FirstOrDefault();
				if (getCompanyAdmin == null)
				{
					return Json(new { isError = true, msg = "This company does not exist" });
				}

				if (getCompanyAdmin.CompanyBranch.Deleted)
				{
					return Json(new { isError = true, msg = "This company has been deactivated, try another active user name" });
				}

				if (superAdmin.UserRole != Utitily.Constants.SuperAdminRole)
				{
					return Json(new { isError = true, msg = "You can't impersonate yourself, you're logged In already." });
				}
				var impersonationRecord = new Impersonation
				{
					CompanyAdminId = getCompanyAdmin?.Id,
					SuperAdminUserId = superAdmin.Id,
					AmTheRealUser = false,
					DateImpersonated = DateTime.Now
				};
				if (impersonationRecord != null)
				{
					_context.Impersonations.Add(impersonationRecord);
					_context.SaveChanges();
					session.Clear();
					var companyAdmin = getCompanyAdmin;
					await _signInManager.SignInAsync(companyAdmin, isPersistent: false).ConfigureAwait(false);
					if (companyAdmin.CompanyBranch?.Company != null)
						companyAdmin.CompanyBranch.Company.CreatedBy = null;
					companyAdmin.Roles = (List<string>)await _userManager.GetRolesAsync(companyAdmin).ConfigureAwait(false);
					companyAdmin.UserRole = Utitily.Constants.CompanyAdminRole;
					var currentUser = JsonConvert.SerializeObject(companyAdmin);
					HttpContext.Session.SetString("myuser", currentUser);
					HttpContext.Session.SetString("isImpersonating", "true");
					var SessionKeyName = companyAdmin.Id;
					if (string.IsNullOrEmpty(session.GetString(SessionKeyName)))
					{
						session.SetString(SessionKeyName, "true");
					}
					else { session.Clear(); }

					return Json(new { isError = false, msg = "Company admin Impersonated  successfully.", data = superAdmin.Id });

				}
			}
			return Json(new { isError = true, msg = "Error Occurred while impersonating." });
		}

		[HttpPost]
		public JsonResult DeleteCompany(Guid companyId)
		{
			if (companyId != Guid.Empty)
			{
				var deleteCompany = _companyHelper.DeleteCompany(companyId);
				if (deleteCompany)
				{
					return Json(new { isError = false, msg = "Company deleted successfully" });
				}
				return Json(new { isError = true, msg = "Unable to deleted Company" });
			}
			return Json(new { isError = true, msg = "Error occurred" });
		}
		public IActionResult LoginUserLogs()
		{
			var loggedUser = _userHelper.GetLogInUserList();

			return View(loggedUser);
		}


		[HttpGet]
		public IActionResult ModuleCost()
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
			ViewBag.Settings = _dropdownHelper.GetCompanySetting();
			var moduleCost = _superAdminHelper.GetModuleCostList();
			return View(moduleCost);
		}


		[HttpPost]
		public JsonResult CreateModuleCost(string costDetails)
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
			if (costDetails != null)
			{
				var moduleCostView = JsonConvert.DeserializeObject<ModuleCostViewModel>(costDetails);
				if (moduleCostView != null)
				{
					var createModuleCost = _superAdminHelper.CreateModuleCost(moduleCostView);
					if (createModuleCost)
					{
						return Json(new { isError = false, msg = "ModuleCost created successfully" });

					}
					return Json(new { isError = true, msg = "Cost already exist" });
				}
				return Json(new { isError = true, msg = "Unable to create ModuleCost" });
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}


		[HttpPost]
		public JsonResult EditmoduleCost(string costDetails)
		{
			if (costDetails != null)
			{
				var costViewModel = JsonConvert.DeserializeObject<ModuleCostViewModel>(costDetails);
				if (costViewModel != null)
				{
					var createModuleCost = _superAdminHelper.EditmoduleCost(costViewModel);
					if (createModuleCost)
					{
						return Json(new { isError = false, msg = "ModuleCost Updated successfully" });
					}
				}
				return Json(new { isError = true, msg = "Unable to update ModuleCost" });
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}


		[HttpGet]
		public JsonResult GetModuleCostByID(string moduleCostId)
		{
			if (moduleCostId != null)
			{
				var moduleCosts = JsonConvert.DeserializeObject<Guid>(moduleCostId);
				var moduleCost = _context.ModuleCosts.Where(c => c.Id == moduleCosts).FirstOrDefault();
				if (moduleCost != null)
				{
					return Json(moduleCost);
				}
			}
			return null;

		}


		[HttpPost]
		public JsonResult DeleteModuleCost(string costDetails)
		{
			if (costDetails != null)
			{
				var costViewModel = JsonConvert.DeserializeObject<ModuleCostViewModel>(costDetails);
				if (costViewModel != null)
				{
					var moduleCost = _superAdminHelper.DeleteModuleCost(costViewModel);
					if (moduleCost)
					{
						return Json(new { isError = false, msg = "ModuleCost Deleted successfully" });

					}
				}
				return Json(new { isError = true, msg = "Unable to Delete ModuleCost" });
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}

		
	}




}
