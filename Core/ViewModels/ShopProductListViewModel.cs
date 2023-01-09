using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
	public class ShopProductListViewModel
	{
		public int Quantity { get; set; }
		public double Amount { get; set; }
		public string? Staff { get; set; }
		public double AmountPerProduct { get; set; }
		public string? Address { get; set; }
		public string? CompanyName { get; set; }
		public string? PhoneNumber { get; set; }
		public string? Email { get; set; }
		public int Qauntity { get; set; }
		public int OldQauntity { get; set; }
		public int NewQuantity { get; set; }
		public int QuantityBought { get; set; }
		public double PricePerUnit { get; set; }
		public int Tag { get; set; }
		public double TotalAmountofQuantityBought { get; set; }
		public int Id { get; set; }
        public string UserId { get; set; }
		public string? Name { get; set; }
		public string? ProductName { get; set; }
		public int? SupplierId { get; set; }
		public string? SupplierName { get; set; }
		public int? UnitId { get; set; }
		public string? UnitName { get; set; }
		public int? ShopCategoryId { get; set; }
		public int? MeasurementunitId { get; set; }
        public string? ProductCategoryName { get; set; }
		public bool Active { get; set; }
		public bool Deleted { get; set; }
		public DateTime DateCreated { get; set; }
	}
}
