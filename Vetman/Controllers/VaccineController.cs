using Core.Db;
using Core.ViewModels;
using Logic.IHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static Logic.AppHttpContext;

namespace Vetman.Controllers
{
	[SessionTimeout]
	public class VaccineController : Controller
	{
		private readonly IUserHelper _userHelper;
		private readonly IVaccineHelper _vaccineHelper;
		private readonly AppDbContext _context;
		private readonly IDropdownHelper _dropdownHelper;
		private IAdminHelper _adminHelper;
		private readonly IEmailHelper _emailHelper;
		private readonly ICustomerPaymentHelper _customerPaymentHelper;


		public VaccineController(AppDbContext context, IUserHelper userHelper, IAdminHelper adminHelper, IEmailHelper emailHelper,
			ICustomerPaymentHelper customerPaymentHelper, IDropdownHelper dropdownHelper, IVaccineHelper vaccineHelper)
		{
			_context = context;
			_adminHelper = adminHelper;
			_userHelper = userHelper;
			_emailHelper = emailHelper;
			_customerPaymentHelper = customerPaymentHelper;
			_dropdownHelper = dropdownHelper;
			_vaccineHelper = vaccineHelper;
		}


		[HttpGet]
		public IActionResult ProductVaccine(int id)
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
			ViewBag.productid = id;
			ViewBag.Product = _dropdownHelper.GetProducts(); ;
			var myProductVaccine = _vaccineHelper.GetProductVaccineList(id);
			return View(myProductVaccine);
		}



		[HttpPost]
		public JsonResult ProductVaccine(string productVaccineDetails)
		{
			if (productVaccineDetails != null)
			{
				var productVaccineViewModel = JsonConvert.DeserializeObject<ProductVaccineViewModel>(productVaccineDetails);
				
				if (productVaccineViewModel != null)
				{
					var checkForProductVaccineName = _context.ProductVaccines.Where(x => x.Name == productVaccineViewModel.Name && productVaccineViewModel.ProductName == x.Name && x.CompanyBranchId == productVaccineViewModel.BranchId).FirstOrDefault();
					if (checkForProductVaccineName != null)
					{
						return Json(new { isError = true, msg = "Vaccine Name Already Exist" });
					}
					var vaccine = _vaccineHelper.CreateProductVaccine(productVaccineViewModel);
					if (vaccine)
					{
						return Json(new { isError = false, msg = "ProductVaccine Created Successfully" });
					}
				}
				return Json(new { isError = true, msg = "Unable to Create ProductVaccine" });
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}



		[HttpPost]
		public JsonResult EditProductVaccine(string productVaccineDetails)
		{
			if (productVaccineDetails != null)
			{
				var productVaccineViewModel = JsonConvert.DeserializeObject<ProductVaccineViewModel>(productVaccineDetails);
				if (productVaccineViewModel != null)
				{
					var createProductVaccine = _vaccineHelper.EditProductVaccine(productVaccineViewModel);
					if (createProductVaccine)
					{
						return Json(new { isError = false, msg = "ProductVaccine Updated successfully" });
					}
				}
				return Json(new { isError = true, msg = "Unable to update ProductVaccine" });
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}

		[HttpPost]
		public JsonResult DeleteProductVaccine(string productVaccineDetails)
		{
			if (productVaccineDetails != null)
			{
				var productVaccineViewModel = JsonConvert.DeserializeObject<ProductVaccineViewModel>(productVaccineDetails);
				if (productVaccineViewModel != null)
				{
					var createProductVaccine = _vaccineHelper.DeleteProductVaccine(productVaccineViewModel);
					if (createProductVaccine)
					{
						return Json(new { isError = false, msg = "ProductVaccine Deleted successfully" });

					}
				}
				return Json(new { isError = true, msg = "Unable to Delete ProductVaccine" });
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}

		[HttpGet]
		public JsonResult GetProductVaccineByID(int productVaccineId)
		{
			if (productVaccineId != 0)
			{
				var productVaccine = _context.ProductVaccines.Where(c => c.Id == productVaccineId).Include(p => p.Product).FirstOrDefault();
				if (productVaccine != null)
				{
					return Json(productVaccine);
				}
			}
			return null;

		}

		[HttpGet]
		public JsonResult ProductVaccineFillter(int productInfoID)
		{
			if (productInfoID != 0)
			{
				var productInfo = _vaccineHelper.GetProductVaccineList(productInfoID);
				if (productInfo != null && productInfo.Count > 0)
				{
					return Json(new { isError = false, data = productInfo });
				}
			}
			return Json(new { isError = true, data = 0 }); ;

		}


		[HttpGet]
		public IActionResult Index()
		{
			var utility = new Utitily();
			ViewBag.Product = _customerPaymentHelper.GetProductDropDown();
			ViewBag.Layout = utility.GetRoleLayout();
			var getTheListOfSubscriber = _customerPaymentHelper.SubscribersList();
			return View(getTheListOfSubscriber);
		}
		[HttpGet]
		public JsonResult GetAllcustomer(string term)
		{
			if (term != null)
			{
				var user = _context.Customers.Where(x => x.FirstName.ToLower().Contains(term.ToLower())).Select(a => new { a.Id, a.FullName }).ToList();
				if (user != null)
				{
					return Json(user);
				}
			}
			return Json("Please Add Customer");
		}
		[HttpPost]
		public JsonResult CreateCustomer(string customerDetails)
		{
			if (customerDetails != null)
			{
				var customerViewModel = JsonConvert.DeserializeObject<VaccineSubscriptionViewModel>(customerDetails);
				if (customerViewModel != null)
				{
					var createCustomer = _customerPaymentHelper.CreateCustomerDetails(customerViewModel);
					if (createCustomer)
					{
						return Json(new { isError = false, msg = "Subscription Created Successfully" });
					}
				}
				return Json(new { isError = true, msg = "Unable to Create Customer" });
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}


		[HttpGet]
		public JsonResult GetVaccineSubscriptionId(int SubDetails)
		{

			try
			{
				if (SubDetails != 0)
				{
					var subToEdit = _customerPaymentHelper.GetVaccineSubscriberData(SubDetails);
					if (subToEdit != null)
					{
						return Json(new { isError = false, data = subToEdit });
					}
				}
				return Json(new { isError = true, msg = "Could not Edit Subscriber." });
			}
			catch (Exception exp)
			{

				throw exp;
			}
		}
		[HttpPost]
		public IActionResult VaccineStatusCancel(string action)
		{
			try
			{
				if (action != null)
				{
					var subscriberData = JsonConvert.DeserializeObject<VaccineSubscriptionViewModel>(action);
					if (subscriberData != null)
					{
						var subDetails2Cancel = _customerPaymentHelper.CancelSubscriber(subscriberData);
						if (subDetails2Cancel != null)
							return Json(new { isError = false, msg = "Subscription Cancelled Successfully." });
					}
				}
				return Json(new { isError = true, msg = "Error occurred" });
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}
		[HttpPost]
		public IActionResult VaccineStatusCompleted(string action)
		{
			try
			{
				if (action != null)
				{
					var subscriberData = JsonConvert.DeserializeObject<VaccineSubscriptionViewModel>(action);
					if (subscriberData != null)
					{
						var subDetails2Cancel = _customerPaymentHelper.CompletedSubscriber(subscriberData);
						if (subDetails2Cancel != null)
							return Json(new { isError = false, msg = "Subscription Completed Successfully." });
					}
				}
				return Json(new { isError = true, msg = "Error occurred" });
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}
		[HttpPost]
		public IActionResult VaccineStatusDeleted(string action)
		{
			try
			{
				if (action != null)
				{
					var subscriberData = JsonConvert.DeserializeObject<VaccineSubscriptionViewModel>(action);
					if (subscriberData != null)
					{
						var subDetails2Cancel = _customerPaymentHelper.DeleteSubscriber(subscriberData);
						if (subDetails2Cancel != null)
							return Json(new { isError = false, msg = "Subscription Deleted Successfully." });
					}
				}
				return Json(new { isError = true, msg = "Error occurred" });
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}
		[HttpPost]
		public JsonResult EditedSubscriber(string editSubMode)
		{
			try
			{
				var subData = JsonConvert.DeserializeObject<VaccineSubscriptionViewModel>(editSubMode);
				if (subData != null)
				{
					var updateSubscriberDetails = _customerPaymentHelper.EditSubDetails(subData);
					if (updateSubscriberDetails != null)
					{
						return Json(new { isError = false, msg = "Vaccine Subscription Edited Successfully." });
					}
				}
				return Json(new { isError = true, msg = "Something Went Wrong." });
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public IActionResult Sales()
		{

			ViewBag.Layout = _userHelper.GetRoleLayout();
			return View();
		}
	}
}
