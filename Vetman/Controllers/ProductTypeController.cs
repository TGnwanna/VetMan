using Core.Db;
using Core.ViewModels;
using Logic.IHelpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Logic.AppHttpContext;

namespace Vetman.Controllers
{
	[SessionTimeout]
	public class ProductTypeController : Controller
	{
		private readonly IAdminHelper _adminHelper;
		private readonly IUserHelper _userHelper;
		private readonly AppDbContext _context;

		public ProductTypeController(AppDbContext context, IUserHelper userHelper, IAdminHelper adminHelper)
		{
			_context = context;

			_userHelper = userHelper;
			_adminHelper = adminHelper;
		}
		//Get Product type
		[HttpGet]
		public IActionResult Index()
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
			var productsTypeList = _adminHelper.GetProductTypeList();
			if (productsTypeList != null && productsTypeList.Count > 0)
			{
				return View(productsTypeList);
			}
			return View(productsTypeList);

		}
		//Post Product type
		[HttpPost]
		public JsonResult ProductTypePostAction(string productTypeDetails)
		{
			if (productTypeDetails != null)
			{
				var productTypeViewModel = JsonConvert.DeserializeObject<ProductTypeViewModel>(productTypeDetails);
				if (productTypeViewModel != null)
				{
					productTypeViewModel.DateCreated = DateTime.Now;

					var createProduct = _adminHelper.CreateProductTypeDetails(productTypeViewModel);
					if (createProduct)
					{
						return Json(new { isError = false, msg = "Product Type Created Successfully" });
					}
				}
				return Json(new { isError = true, msg = "Unable to Create Product Type" });
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}

		[HttpGet]
		public JsonResult EditedProductType(int productTypeId)
		{
			try
			{
				if (productTypeId > 0)
				{
					var prosType = _adminHelper.EditProductTypeDetails(productTypeId);
					if (prosType != null)
					{
						return Json(new { isError = false, data = prosType });
					}
				}
				return Json(new { isError = true, msg = "Could not Edit Product Type." });
			}
			catch (Exception exp)
			{

				throw exp;
			}
		}
		[HttpPost]
		public JsonResult EditedProductType(string productType)
		{
			try
			{

				var productTyeData = JsonConvert.DeserializeObject<ProductTypeViewModel>(productType);
				if (productTyeData != null)
				{
					var updateProductTypeDetails = _adminHelper.EditProductTypeDetails(productTyeData);
					if (updateProductTypeDetails != null)
					{
						return Json(new { isError = false, msg = "Product Type Edited Successfully." });
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
		public JsonResult DeleteProductType(string productType)
		{
			try
			{
				if (productType != null)
				{
					var productData = JsonConvert.DeserializeObject<ProductTypeViewModel>(productType);
					if (productData != null)
					{
						var updateProductDetails = _adminHelper.DeleteProductTypeDetails(productData);
						if (updateProductDetails != null)
						{
							return Json(new { isError = false, msg = "Product type Deleted Successfully." });
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
	}

}
