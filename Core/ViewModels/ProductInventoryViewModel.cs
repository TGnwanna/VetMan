using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Enums.VetManEnums;

namespace Core.ViewModels
{
    public class ProductInventoryViewModel
    {
        public int? MeasurementunitId { get; set; }
        public int? ShopCategoryId { get; set; }
        public int? SupplierId { get; set; }
        public string? SupplierName { get; set; }
		public ProductActivity ProductActivity { get; set; }
		public double SellingPrice { get; set; }
		public int Quantity { get; set; }
		public int ProductInventoryId { get; set; }
		public int OldQauntity { get; set; }
		public string? UserId { get; set; }
		public string? ProductName { get; set; }
		public int NewQuantity { get; set; }
		public double TotalAmountPaid { get; set; }
        public double AmountPerProduct { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? AccessName { get; set; }
        public string? Tag { get; set; }
        public bool Active { get; set; }
        public bool IsNewProduct { get; set; }
        public bool Deleted { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid? CompanyBranchId { get; set; }
    }
}
