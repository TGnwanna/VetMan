using Core.Db;
using Core.ViewModels;
using Logic.IHelpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Logic.AppHttpContext;

namespace Vetman.Controllers
{
	[SessionTimeout]
	public class ShopProductsController : Controller
	{
		private readonly IAdminHelper _adminHelper;
		private readonly ISalesHelper _salesHelper;
		private readonly IUserHelper _userHelper;
		private readonly AppDbContext _context;

		public ShopProductsController(AppDbContext context, IUserHelper userHelper, IAdminHelper adminHelper, ISalesHelper salesHelper)
		{
			_context = context;
			_userHelper = userHelper;
			_adminHelper = adminHelper;
			_salesHelper = salesHelper;
		}

		[HttpGet]
		public IActionResult Index()
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
			var getProductCategoryList = _adminHelper.GetProductCategoryList();
			if (getProductCategoryList != null && getProductCategoryList.Count > 0)
			{
				return View(getProductCategoryList);
			}
			return View(getProductCategoryList);

		}
		//Post Product Category
		[HttpPost]
		public JsonResult ProductCategory(string productCatDetails)
		{
			if (productCatDetails != null)
			{
				var shopProductViewModel = JsonConvert.DeserializeObject<ShopProductViewModel>(productCatDetails);
				if (shopProductViewModel != null)
				{
                    var checkExistingProductCategoryName = _context.ShopCategories.Where(x => x.Name == shopProductViewModel.Name && x.Active).FirstOrDefault();
                    if (checkExistingProductCategoryName != null)
                    {
                        return Json(new { isError = true, msg = "Product Category Name Already Exist" });
                    }
                    var createShopProduct = _adminHelper.CreateShopProductDetails(shopProductViewModel);
                    if (createShopProduct)
					{
						
						return Json(new { isError = false, msg = "Product Category Created Successfully" });
					}
				}
				return Json(new { isError = true, msg = "Unable to Create Product Category" });
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}
		[HttpGet]
		public JsonResult EditedShopProduct(int productCatId)
		{
			try
			{
				if (productCatId > 0)
				{
					var productCat = _adminHelper.EditProductCatDetails(productCatId);
					if (productCat != null)
					{
						return Json(new { isError = false, data = productCat });
					}
				}
				return Json(new { isError = true, msg = "Could not Edit Product Category." });
			}
			catch (Exception exp)
			{

				throw exp;
			}
		}

		[HttpPost]
		public JsonResult ProductCatToSave(string productCategory)
		{
			try
			{
				var productCatDetails = JsonConvert.DeserializeObject<ShopProductViewModel>(productCategory);
				if (productCatDetails != null)
				{
					var updateProductCatDetails = _adminHelper.EditProductCAtDetails(productCatDetails);
					if (updateProductCatDetails)
					{
						return Json(new { isError = false, msg = "Product Category Edited Successfully." });
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
		public JsonResult DeleteProductCategory(string productCat)
		{
			try
			{
				if (productCat != null)
				{
					var productCatDetails = JsonConvert.DeserializeObject<ShopProductViewModel>(productCat);
					if (productCatDetails != null)
					{
						var updateProductDetails = _adminHelper.DeleteProductCatDetails(productCatDetails);
						if (updateProductDetails)
						{
							return Json(new { isError = false, msg = "Product Category Deleted Successfully." });
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
		public IActionResult ProductInventory()
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
			var supplier = _adminHelper.GetSuppliers();
			ViewBag.Supplier = supplier;
			ViewBag.Units = _adminHelper.GetUnits();
			ViewBag.ProductCategories = _adminHelper.GetProductCategories();
			ViewBag.Products = _adminHelper.GetShopProducts();

			return View();
		}

		public IActionResult AddProduct(string addShopProduct)
		{
			if (addShopProduct != string.Empty)
			{
				var data = JsonConvert.DeserializeObject<ProductInventoryViewModel>(addShopProduct);
				if (data != null && data.IsNewProduct)
				{


					if (data.Name == "")
						return Json(new { isError = true, msg = "Product Name is required" });

					if (data.ShopCategoryId == 0)
						return Json(new { isError = true, msg = "Please select category" });

					if (data.MeasurementunitId == 0)
						return Json(new { isError = true, msg = "Please select the unit" });
				}


				if (data.SupplierId == 0)
					return Json(new { isError = true, msg = "Please select supplier" });

				if (data.Quantity == 0)
					return Json(new { isError = true, msg = "Please enter quantity" });

				if (data.AmountPerProduct == 0)
					return Json(new { isError = true, msg = "Please enter Amount Per Product" });

				if (data.TotalAmountPaid == 0)
					return Json(new { isError = true, msg = "Please enter Total Amount Paid" });

				if (data != null)
				{
					if (!data.IsNewProduct)
					{

						var isUpdated = _salesHelper.UpdateSalesProduct(data);
						if (isUpdated)
							return Json(new { isError = false, msg = "Sales Add Successfully" });
						else
							return Json(new { isError = true, msg = "Failed! Please try again shortly " });
					}
					else
					{
						var checkProductInventoryName = _salesHelper.CheckInventoryName(data);
						if (checkProductInventoryName)
							return Json(new { isError = true, msg = "Product Name Already Exist " });
						var isCreated = _salesHelper.CreateSalesProduct(data);
						if (isCreated)
							return Json(new { isError = false, msg = "Sales Add Successfully" });
						else
							return Json(new { isError = true, msg = "Failed! Please try again shortly " });
					}
				}
			}
			return Json(new { isError = true, msg = "Please fill the form correctly" });
		}

		public IActionResult GetProductDetail(int id, int supplierId)
		{
			var detail = _salesHelper.GetProductDetail(id, supplierId);
			if (detail != null)
			{
				return Json(new { isError = false, data = detail });
			}
			return Json(new { isError = true, msg = "Supplier not attached to any Product " });
		}




		public IActionResult Views()
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
            var supplier = _adminHelper.GetSuppliers();
            ViewBag.Supplier = supplier;
            ViewBag.Units = _adminHelper.GetUnits();
            ViewBag.ProductCategories = _adminHelper.GetProductCategories();
            ViewBag.Products = _adminHelper.GetShopProducts();
            var ShopProductList = _adminHelper.GetShopProductList();
			if (ShopProductList != null && ShopProductList.Count > 0)
			{
				return View(ShopProductList);
			}
			return View(ShopProductList);
		}

		[HttpGet]
		public JsonResult EditedproductInventory(int id)
		{
            ViewBag.Layout = _userHelper.GetRoleLayout();
            var supplier = _adminHelper.GetSuppliers();
            ViewBag.Supplier = supplier;
            ViewBag.Units = _adminHelper.GetUnits();
            ViewBag.ProductCategories = _adminHelper.GetProductCategories();
            if (id != 0)
			{
                var productInventoryToBeEdited = _adminHelper.GetProductInventoryById(id);
                if (productInventoryToBeEdited != null)
                {
                    return Json(productInventoryToBeEdited);
                }
            }
            return Json(new { isError = true, msg = "Error occured" });
        }

        [HttpPost]
        public JsonResult EditedShopProductInventory(string shopdetails)
        {
            if (shopdetails != null)
            {
                var shopProductListViewModel = JsonConvert.DeserializeObject<ShopProductListViewModel>(shopdetails);
                if (shopProductListViewModel != null)
                {
                    var shopProduct = _adminHelper.EditProductInventory(shopProductListViewModel);
                    if (shopProduct != null)
                    {
                        return Json(new { isError = false, msg = "Inventory Updated Successfully" });
                    }
                    return Json(new { isError = true, msg = "Unable To Updated Inventory" });
                }
            }
            return Json(new { isError = true, msg = "Error Occured" });
        }

        [HttpPost]
        public JsonResult DeleteShopProductInventoty(int id)
        {
            if (id != 0)
            {
                var shopProduct = _adminHelper.DeleteProductInventory(id);
                if (shopProduct != null)
                {
                    return Json(new { isError = false, msg = "Inventory  deleted Successfully" });
                }
                return Json(new { isError = true, msg = "Unable To delete Inventory" });
            }
            return Json(new { isError = true, msg = "Error occured" });
        }
    }
}
