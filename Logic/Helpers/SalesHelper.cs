using Core.Db;
using Core.Models;
using Core.ViewModels;
using Logic.IHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using static Core.Enums.VetManEnums;

namespace Logic.Helpers
{
	public class SalesHelper : ISalesHelper
	{
		private readonly AppDbContext _context;
		private readonly IUserHelper _userHelper;

		public SalesHelper(AppDbContext context, IUserHelper userHelper)
		{
			_context = context;
			_userHelper = userHelper;
		}


		public bool CreateSalesProduct(ProductInventoryViewModel data)
		{
			if (data != null)
			{
				var loggedInUser = Utitily.GetCurrentUser();
				if (loggedInUser.Id == null)
				{
					loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;
				}
				if (loggedInUser != null)
				{
					var product = new ProductInventory()
					{
						Name = data.Name,
						Active = true,
						CompanyBranchId = loggedInUser.CompanyBranchId,
						DateCreated = DateTime.Now,
						Deleted = false,
						Amount = data.TotalAmountPaid,
						MeasurementunitId = data.MeasurementunitId,
						ShopCategoryId = data.ShopCategoryId,
						SupplierId = data.SupplierId,
						Quantity = data.Quantity,
						AmountPerProduct = data.AmountPerProduct,
					};
					_context.Add(product);
					_context.SaveChanges();

					var log = new ProductLog()
					{
						Active = true,
						DateCreated = DateTime.Now,
						Deleted = false,
						Qauntity = product.Quantity,
						NewQuantity = product.Quantity,
						OldQauntity = product.Quantity,
						CompanyBranchId = loggedInUser.CompanyBranchId,
						Name = product.Name,
						ProductActivity = ProductActivity.Newstock,
						ProductInventoryId = product.Id,
						SellingPrice = product.AmountPerProduct,
						UserId = loggedInUser.Id
					};
					_context.Add(log);
					_context.SaveChanges();
					return true;
				}
			}
			return false;
		}
		public bool CheckInventoryName(ProductInventoryViewModel data)
		{
			if (data.Name != null)
			{
				var loggedInuser = Utitily.GetCurrentUser();
				if (loggedInuser.Id == null)
					loggedInuser = _userHelper.UpdateSessionAsync(loggedInuser.UserName)?.Result;

				if (loggedInuser != null)
				{
					var nameChecks = _context.ProductInventories.Where(f => f.Name == data.Name && f.CompanyBranchId == loggedInuser.CompanyBranchId).FirstOrDefault();
					if (nameChecks != null)
						return true;
				}
			}
			return false;
		}

		public bool UpdateSalesProduct(ProductInventoryViewModel data)
		{
			if (data != null)
			{
				var loggedInUser = Utitily.GetCurrentUser();
				if (loggedInUser.Id == null)
				{
					loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

				}
				if (loggedInUser != null)
				{
					var existingProduct = _context.ProductInventories.Where(x => x.Id == data.Id && x.SupplierId == data.SupplierId &&
							x.CompanyBranchId == loggedInUser.CompanyBranchId).FirstOrDefault();
					if (existingProduct != null)
					{
						var productLog = new ProductLog()
						{
							Active = true,
							CompanyBranchId = loggedInUser.CompanyBranchId,
							DateCreated = DateTime.Now,
							Deleted = false,
							Name = existingProduct.Name,
							OldQauntity = existingProduct.Quantity,
							NewQuantity = data.Quantity,
							ProductInventoryId = existingProduct.Id,
							Qauntity = existingProduct.Quantity + data.Quantity,
							UserId = loggedInUser.Id,
							SellingPrice = data.AmountPerProduct,
							ProductActivity = ProductActivity.Restock
						};
						_context.Add(productLog);
						existingProduct.Amount += data.TotalAmountPaid;
						existingProduct.AmountPerProduct = data.AmountPerProduct;
						existingProduct.Quantity = productLog.Qauntity;
						_context.Update(existingProduct);
						_context.SaveChanges();
						return true;
					}
				}
			}
			return false;
		}

		public List<SaleLogsViewModel> GetCustomerDropDown()
		{
			var loggedInUser = Utitily.GetCurrentUser();
			var common = new SaleLogsViewModel()
			{
				CustomerId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
				CustomerName = "-- Select --"
			};
			var customers = _context.Customers.Where(x => !x.Deleted && x.CompanyBranchId == loggedInUser.CompanyBranchId)
				.Select(s => new SaleLogsViewModel()
				{
					CustomerId = s.Id,
					CustomerName = s.FullName,
				}).ToList();
			customers.Insert(0, common);
			return customers;
		}

		public ProductInventory GetProductDetail(int id, int supplierId)
		{
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

			}
			if (loggedInUser != null)
			{
				if (id > 0 && supplierId > 0)
				{
					var getProductDetails = _context.ProductInventories.Where(x => x.Id == id && x.SupplierId == supplierId && x.CompanyBranchId == loggedInUser.CompanyBranchId)?.FirstOrDefault();
					if(getProductDetails != null)
						return getProductDetails;
				}
			}
			return null;
		}
		public List<SaleLogsViewModel> GetItemList()
		{
			var items = new List<SaleLogsViewModel>();
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser != null)
			{
				var day = DateTime.Now.AddDays(-7);

				items = _context.SalesLogs
				.Where(x => !x.Deleted && x.CompanyBranchId == loggedInUser.CompanyBranchId && x.DateCreated >= day)
					.Include(p => p.User).Include(s => s.Customer)
				.Select(s => new SaleLogsViewModel()
				{
					Id = s.Id,
					DateCreated = s.DateCreated,
					TotalAmountPaid = s.TotalPaid,
					CustomerId = (Guid)s.CustomerId,
					UserId = s.UserId,
					SaleDetails = s.SalesDetail,
					Customer = s.Customer.FullName,
					Staff = s.User.FullName,
				}).ToList();

			}
			return items;
		}
		public List<SaleLogsViewModel> GetItemListFromSearch(Guid customerId, DateTime dateGetFrom, DateTime dateToEnd)
		{
			var itemLists = new List<SaleLogsViewModel>();
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser != null)
			{
				
				var customer = customerId;
				if (dateGetFrom != DateTime.MinValue && dateToEnd != DateTime.MinValue && customer != Guid.Empty)
				{
					 itemLists = _context.SalesLogs
					.Where(x => !x.Deleted && x.CompanyBranchId == loggedInUser.CompanyBranchId && x.DateCreated >= dateGetFrom && x.CustomerId == customer
							&& dateToEnd >= x.DateCreated).Include(p => p.User).Include(s => s.Customer)
					.Select(s => new SaleLogsViewModel()
					{
					
						DateFromD = s.DateCreated.ToLongDateString(),
						Id = s.Id,
						TotalAmountPaid = s.TotalPaid,
						CustomerId = (Guid)s.CustomerId,
						UserId = s.UserId,
						SaleDetails = s.SalesDetail,
						Customer = s.Customer.FullName,
						Staff = s.User.FullName,
					}).ToList();
				} 
				else
				{
                    itemLists = _context.SalesLogs
					.Where(x => !x.Deleted && x.CompanyBranchId == loggedInUser.CompanyBranchId && x.DateCreated >= dateGetFrom && x.DateCreated <= dateToEnd)
						.Include(p => p.User).Include(s => s.Customer)
					.Select(s => new SaleLogsViewModel()
					{
						DateFromD = s.DateCreated.ToLongDateString(),
						Id = s.Id,
						TotalAmountPaid = s.TotalPaid,
						CustomerId = (Guid)s.CustomerId,
						UserId = s.UserId,
						SaleDetails = s.SalesDetail,
						Customer = s.Customer.FullName,
						Staff = s.User.FullName,
					}).ToList();
				}
			}
			return itemLists;
		}
		
		public SalesPaymentViewModel CreateSelectedProducts(List<SalesViewModel> sales, string customerId, string totalAmountPaid)
		{
			var salesPaymentViewModel = new SalesPaymentViewModel();
			var customerIdGuid = Guid.Parse(customerId);
			var total = totalAmountPaid.Replace(",","");
			var totalAmount = Convert.ToDouble(total);
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

			}
			if (sales.Count > 0  && customerIdGuid != Guid.Empty)
			{
				double TotalPaid = 0;
                var salesDetail = JsonConvert.SerializeObject(sales);
				foreach (var item in sales)
				{
					var product = _context.ProductInventories.Where(x => x.Id == item.Id && x.CompanyBranchId == loggedInUser.CompanyBranchId).FirstOrDefault();
					if (product != null)
					{
						var oldQty = product.Quantity;
						var amountToPay = (item.Quantity * product.AmountPerProduct) - item.Discount;
						TotalPaid += amountToPay;
						var qtyLeft = product.Quantity - item.Quantity;
						product.Quantity = qtyLeft;
						var log = new ProductLog()
						{
							Active = true,
							DateCreated = DateTime.Now,
							Deleted = false,
							Qauntity = qtyLeft,
							NewQuantity = qtyLeft,
							OldQauntity = oldQty,
							CompanyBranchId = loggedInUser.CompanyBranchId,
							Name = product.Name,
							ProductActivity = ProductActivity.Sale,
							ProductInventoryId = product.Id,
							SellingPrice = product.AmountPerProduct,
							UserId = loggedInUser.Id
						};
						_context.Update(product);
						_context.Add(log);
						_context.SaveChanges();
                    }
				}
				var productLog = new SalesLog()
				{
					CompanyBranchId = loggedInUser.CompanyBranchId,
					DateCreated = DateTime.Now,
					SalesDetail = salesDetail,
					CustomerId = customerIdGuid,
					Deleted = false,
					UserId = loggedInUser.Id,
					Active = true,
					TotalPaid = TotalPaid
                };
				_context.SalesLogs.Add(productLog);
				_context.SaveChanges();
				var detailsHistory = new List<SaleDetail>();
				foreach (var item in sales)
				{
					var sale = new SaleDetail()
					{
						Discount = item.Discount,
						InventoryId = item.Id,
						Quantity = item.Quantity,
						SalesLogId = productLog.Id,
						SellingPrice = item.UnitPrice
					};
					detailsHistory.Add(sale);
				}
				_context.AddRange(detailsHistory);
				_context.SaveChanges();
				 salesPaymentViewModel = new SalesPaymentViewModel()
				{
					Amount = TotalPaid,
					SalesLogsId = productLog.Id,
				};
				return salesPaymentViewModel;
			}
			return salesPaymentViewModel;
		}

        public SaleLogsViewModel GetCustomerSalesHistory(int id)
        {
            var detail = new SaleLogsViewModel();
            var loggedInUser = Utitily.GetCurrentUser();
            if (loggedInUser != null)
            {
				var item = _context.SalesLogs.Where(x => !x.Deleted && x.Id == id && x.CompanyBranchId == loggedInUser.CompanyBranchId)
				 .Include(p => p.User).Include(s => s.Customer).FirstOrDefault();
				var details = JsonConvert.DeserializeObject<List<SalesViewModel>>(item?.SalesDetail);
				var paidItems = new List<SalesDetailViewModel>();
				foreach (var data in details)
				{
					var product = _context.ProductInventories.Where(x => x.Id == data.Id).Include(p => p.Supplier).Include(c => c.Measurementunit).FirstOrDefault();
					if (product != null)
					{
						var paidItem = new SalesDetailViewModel()
						{
							ProductId = product.Id,
							ProductName = product?.Name,
                            Measurementunit = product.Measurementunit?.Name,
							SupplierId = product.SupplierId,
							SupplierName = product.Supplier?.Name,
							Discount = data.Discount,
							UnitPrice = data.UnitPrice,
							Quantity = data.Quantity
						};
						paidItems.Add(paidItem);
					}
				}
				var sale = new SaleLogsViewModel()
				{
                    Id = item.Id,
                    DateCreated = item.DateCreated,
                    TotalAmountPaid = item.TotalPaid,
                    CustomerId = item.CustomerId,
                    UserId = item.UserId,
                    SaleDetailsModel = paidItems,
                    Customer = item.Customer?.FullName,
                    Staff = item.User?.FullName,
					Phone = item.Customer?.PhoneNumber,
                    CustomerAddress = item.Customer.Address
                };
                
				return sale;
            }
            return detail;
        }

        public List<PaymentViewModel> GetPaymentList()
        {
            var items = new List<PaymentViewModel>();
            var loggedInUser = Utitily.GetCurrentUser();
            if (loggedInUser != null)
            {
                items = _context.SalesPayments
                .Where(x => x.CompanyBranchId == loggedInUser.CompanyBranchId)?.Include(p => p.SalesLog)?
                   .Include(d => d.PaymentMeans)?.Include(s => s.SalesLog.User)?.Include(s => s.SalesLog.Customer)
                .Select(s => new PaymentViewModel()
                {
                    Id = s.Id,
                    AmountPaid = s.AmountPaid,
                    Balance = s.Balance,
                    CustomerId = (Guid)s.SalesLog.CustomerId,
                    StaffId = s.SalesLog.UserId,
                    CustomerName = s.SalesLog.Customer.FullName,
                    OrderId = s.OrderId,
					PaymentDate = s.PaymentDate,
					PaymentMethod = s.PaymentMeans.Name,
                    StaffName = s.SalesLog.User.FullName,
                }).ToList();

            }
            return items;
        }

    }
}
