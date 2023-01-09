using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
	public class SalesDetailViewModel
	{
		public string ProductName { get; set; }
		public string SupplierName { get; set; }
		public string Measurementunit { get; set; }
		public double Discount { get; set; }
		public double UnitPrice { get; set; }
		public int ProductId { get; set; }
		public int? SupplierId { get; set; }
		public int? Quantity { get; set; }
	}
}
