using Core.Db;
using Core.ViewModels;
using Logic.IHelpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Logic.AppHttpContext;

namespace Vetman.Controllers
{
	[SessionTimeout]
	public class ProductController : Controller
	{

		private readonly IAdminHelper _adminHelper;
		private readonly IUserHelper _userHelper;
		private readonly IBookingHelper _bookingHelper;
		private readonly AppDbContext _context;

		public ProductController(AppDbContext context, IUserHelper userHelper, IAdminHelper adminHelper, IBookingHelper bookingHelper)
		{
			_context = context;

			_userHelper = userHelper;
			_adminHelper = adminHelper;
			_bookingHelper = bookingHelper;
		}
		[HttpGet]
		public IActionResult Index()
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
			ViewBag.ProductType = _bookingHelper.GetProductTypeDropDown();
			var productsList = _adminHelper.GetProductList();
			if (productsList != null && productsList.Count > 0)
			{
				return View(productsList);
			}
			return View(productsList);

		}
		[HttpPost]
		public JsonResult ProductPostAction(string productDetails)
		{
			if (productDetails != null)
			{
				var productViewModel = JsonConvert.DeserializeObject<ProductViewModel>(productDetails);
				if (productViewModel != null)
				{
					var createProduct = _adminHelper.CreateProductDetails(productViewModel);
					if (createProduct)
					{
						return Json(new { isError = false, msg = "Product Created Successfully" });
					}
				}
				return Json(new { isError = true, msg = "Unable to Create Product" });
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}

		[HttpGet]
		public JsonResult EditedProduct(int productId)
		{
			if (productId > 0)
			{
				var pros = _adminHelper.EditProductDetails(productId);
				if (pros != null)
				{
					return Json(new { isError = false, data = pros });
				}
			}
			return Json(new { isError = true, msg = "Could not Edit Product." });
		}
		[HttpPost]
		public JsonResult EditedProduct(string product)
		{
			var productData = JsonConvert.DeserializeObject<ProductViewModel>(product);
			if (productData != null)
			{
				var updateProductDetails = _adminHelper.EditProductDetails(productData);
				if (updateProductDetails != null)
				{
					return Json(new { isError = false, msg = "Product Edited Successfully." });
				}
			}
			return Json(new { isError = true, msg = "Something Went Wrong." });
		}
		[HttpPost]
		public JsonResult DeleteProduct(string product)
		{
			var productData = JsonConvert.DeserializeObject<ProductViewModel>(product);
			if (productData != null)
			{
				var updateProductDetails = _adminHelper.DeleteProductDetails(productData);
				if (updateProductDetails != null)
				{
					return Json(new { isError = false, msg = "Product Deleted Successfully." });
				}
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}
	}
}
