using Core.Db;
using Core.ViewModels;
using Logic.IHelpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Logic.AppHttpContext;

namespace Vetman.Controllers
{
	[SessionTimeout]
	public class SuppliersController : Controller
	{
		private AppDbContext _context;
		private IAdminHelper _adminHelper;
		private IUserHelper _userHelper;
		private readonly IEmailHelper _emailHelper;
		private readonly IDropdownHelper _dropdownHelper;

		public SuppliersController(AppDbContext context, IUserHelper userHelper, IAdminHelper adminHelper, IEmailHelper emailHelper, IDropdownHelper dropdownHelper)
		{
			_context = context;
			_adminHelper = adminHelper;
			_userHelper = userHelper;
			_emailHelper = emailHelper;
			_dropdownHelper = dropdownHelper;
		}

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
					ViewBag.Layout = _userHelper.GetRoleLayout();
					var supplierList = _userHelper.GetSupplierList();
					return View(supplierList);
				}
				return RedirectToAction("CantAccess", "Admin");
			}
			catch (Exception)
			{

				throw;
			}

		}
		[HttpPost]
		public JsonResult RegisterSupplier(string supplierDetails)
		{
			try
			{
				if (supplierDetails != null)
				{
					var supplierViewModel = JsonConvert.DeserializeObject<SupplierViewModel>(supplierDetails);
					if (supplierViewModel != null)
					{
						var phoneNumberValidation = _userHelper.CheckSupplierPhoneNumber(supplierViewModel);
						if (!phoneNumberValidation)
						{
							return Json(new { isError = true, msg = "PhoneNumber Already used by Another Supplier" });
						}
						var createSupplier = _userHelper.CreateSupplierDetails(supplierViewModel);
						if (createSupplier)
						{
							return Json(new { isError = false, msg = "Supplier Registered Successfully" });
						}
					}
					return Json(new { isError = true, msg = "Unable to Registered Supplier" });
				}
				return Json(new { isError = true, msg = "Error Occurred" });
			}
			catch (Exception exp)
			{

				throw exp;
			}

		}
		[HttpGet]
		public JsonResult EditedSupplier(int id)
		{
			if (id > 0)
			{
				var getSupDetails = _userHelper.EditSupplier(id);
				if (getSupDetails != null)
				{
					return Json(new { isError = false, data = getSupDetails });
				}
			}
			return Json(new { isError = true, msg = " you Can't Edit Supplier's Details." });
		}
		[HttpPost]
		public JsonResult SaveEditedSupplier(string supplierDetails)
		{
			try
			{
				var supplierData = JsonConvert.DeserializeObject<SupplierViewModel>(supplierDetails);
				if (supplierData != null)
				{
					var supplier2Update = _userHelper.SupplierEditedDetails(supplierData);
					if (supplier2Update != null)
					{
						return Json(new { isError = false, msg = "Supplier Edited Successfully." });
					}
				}
				return Json(new { isError = true, msg = "Something Went Wrong." });
			}
			catch (Exception)
			{

				throw;
			}

		}

		[HttpPost]
		public JsonResult DeleteSupplier(string supplierDetails)
		{
			try
			{
				if (supplierDetails != null)
				{
					var supplierData = JsonConvert.DeserializeObject<SupplierViewModel>(supplierDetails);
					if (supplierData != null)
					{
						var supplierDetails2Delete = _userHelper.DeleteSupplierDetails(supplierData);
						if (supplierDetails2Delete != null)
							return Json(new { isError = false, msg = "Supplier Deleted Successfully." });
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
