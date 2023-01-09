using Core.ViewModels;
using Logic.IHelpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Logic.AppHttpContext;

namespace Vetman.Controllers
{
	[SessionTimeout]
	public class SalesController : Controller
	{
		private readonly IUserHelper _userHelper;
		private IAdminHelper _adminHelper;
		private readonly IBookingHelper _bookingHelper;
		private readonly IDropdownHelper _dropdownHelper;
		private readonly ISalesHelper _salesHelper;

		public SalesController(IUserHelper userHelper, IAdminHelper adminHelper, IBookingHelper bookingHelper, IDropdownHelper dropdownHelper, ISalesHelper salesHelper)
		{
			_userHelper = userHelper;
			_adminHelper = adminHelper;
			_bookingHelper = bookingHelper;
			_dropdownHelper = dropdownHelper;
			_salesHelper = salesHelper;
		}
		public IActionResult Index()
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
			ViewBag.Customer = _salesHelper.GetCustomerDropDown();
			var itemsList = _salesHelper.GetItemList();
			return View(itemsList);
		}
		public JsonResult Itemfilter(Guid customerId, DateTime dateGetFrom, DateTime dateToEnd)
		{
			if (dateGetFrom != DateTime.MinValue && dateToEnd != DateTime.MinValue)
			{
				var getRequiredData = _salesHelper.GetItemListFromSearch(customerId, dateGetFrom, dateToEnd);
				if (getRequiredData != null && getRequiredData.Count > 0)
				{
					return Json(new { isError = false, data = getRequiredData });
				}
			}
			return Json(new { isError = true });
		}
		public IActionResult AddSales()
		
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
			ViewBag.ShopProduct = _adminHelper.GetShopProducts();
			ViewBag.Customers = _bookingHelper.GetCustomers();
			return View();
		}

		public JsonResult GetProductDetail(int id)
		{
			var productData = _dropdownHelper.GetProductDetails(id);
			if (productData != null)
				return Json(new { isError = false, data = productData });

			return Json(new { isError = true });
		}


		public IActionResult SaveSales(string salesDetails, string customerId, string totalAmountPaid)
		{
			if (salesDetails != null && salesDetails != "[]")
			{
				var selectedProducts = JsonConvert.DeserializeObject<List<SalesViewModel>>(salesDetails);
				if (selectedProducts.Count > 0)
				{
					var saveSelectedProduct = _salesHelper.CreateSelectedProducts(selectedProducts, customerId, totalAmountPaid);
					if (saveSelectedProduct != null)
						return Json(new { isError = false, msg = "Saved Successfully", data = saveSelectedProduct });
				}
			}
			return Json(new { isError = true, msg = "Please add product for customer.", });
		}
		public IActionResult CustomerSalesHistory(int id)
		{
			if (id > 0)
			{
				var itemsList = _salesHelper.GetCustomerSalesHistory(id);
				if (itemsList != null)
				{
					return Json(new { isError = false, data = itemsList });
				}
			}
			return Json(new { isError = true });
		}

        public IActionResult Payments()
        {
            ViewBag.Layout = _userHelper.GetRoleLayout();
            var itemsList = _salesHelper.GetPaymentList();
            return View(itemsList);
        }
    }
}
