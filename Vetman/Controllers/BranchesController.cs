using Core.Db;
using Core.ViewModels;
using Logic.IHelpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Logic.AppHttpContext;

namespace Vetman.Controllers
{
	[SessionTimeout]
	public class BranchesController : Controller
	{
		private readonly ICompanyHelper _companyHelper;
		private readonly IUserHelper _userHelper;
		IEmailHelper _emailHelper;
		private readonly AppDbContext _context;

		public BranchesController(AppDbContext context, IUserHelper userHelper, ICompanyHelper companyHelper, IEmailHelper emailHelper)
		{
			_context = context;
			_emailHelper = emailHelper;
			_userHelper = userHelper;
			_companyHelper = companyHelper;
		}
		// Get the list of all the company branches
		[HttpGet]
		public IActionResult Index()
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
					var utility = new Utitily();
					ViewBag.Layout = utility.GetRoleLayout();
					var companyBranchList = _companyHelper.GetCompanyBranchList();
					return View(companyBranchList);
				}
				return RedirectToAction("CantAccess", "Admin");
			}
			catch (Exception exp)
			{
				throw exp;
			}


		}
		[HttpPost]
		public JsonResult CompanyBranchPostAction(string branchDetails)
		{
			if (branchDetails != null)
			{
				var companyBranchViewModel = JsonConvert.DeserializeObject<CompanyBranchViewModel>(branchDetails);
				if (companyBranchViewModel != null && companyBranchViewModel.Name != "" && companyBranchViewModel.Address != "")
				{
					var checkForCompanyBranchName = _context.CompanyBranches.Where(x => x.Name == companyBranchViewModel.Name && x.Address == companyBranchViewModel.Address ).FirstOrDefault();
					if (checkForCompanyBranchName != null)
					{
						return Json(new { isError = true, msg = "CompanyBranch Name belongs to another Branch" });
					}
					var createProduct = _companyHelper.CreateCompanyBranchDetails(companyBranchViewModel);
					if (createProduct)
						return Json(new { isError = false, msg = "Branch created successfully" });
				}
				return Json(new { isError = true, msg = "Unable to create Branch." });
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}
		[HttpGet]
		public JsonResult EditedCompanyBranch(Guid CompanyId)
		{
			try
			{
				if (CompanyId != null)
				{
					var branch2Edit = _companyHelper.EditCompanyBranchDetails(CompanyId);
					if (branch2Edit != null)
						return Json(new { isError = false, data = branch2Edit });
				}
				return Json(new { isError = true, msg = "Could not edit company branch." });
			}
			catch (Exception exp)
			{

				throw exp;
			}
		}
		[HttpPost]
		public JsonResult EditedCompanyBranch(string branch)
		{
			try
			{

				var branchData = JsonConvert.DeserializeObject<CompanyBranchViewModel>(branch);
				if (branchData != null)
				{
					var updateProductDetails = _companyHelper.EditedCompanyBranchDetails(branchData);
					if (updateProductDetails != null)
						return Json(new { isError = false, msg = "Branch Edited Successfully." });
				}
				return Json(new { isError = true, msg = "Something went wrong." });
			}
			catch (Exception exp)
			{
				throw exp;
			}
		}

		[HttpPost]
		public JsonResult DeleteCompanyBranch(string branchDetails)
		{
			try
			{
				if (branchDetails != null)
				{
					var companyBranchData = JsonConvert.DeserializeObject<CompanyBranchViewModel>(branchDetails);
					if (companyBranchData != null)
					{
						var branchDetails2Delete = _companyHelper.DeleteCompanyDetails(companyBranchData);
						if (branchDetails2Delete != null)
							return Json(new { isError = false, msg = "Branch Deleted Successfully." });
					}
				}
				return Json(new { isError = true, msg = "Something went wrong." });
			}
			catch (Exception exp)
			{
				throw exp;
			}
		}
	}
}
