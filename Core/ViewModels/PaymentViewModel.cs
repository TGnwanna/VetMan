using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
	public class PaymentViewModel
    {
		public int Id { get; set; }
		public Guid CustomerId { get; set; }
		public string CustomerName { get; set; }
		public string StaffName { get; set; }
		public string StaffId { get; set; }
		public int OrderId { get; set; }
		public DateTime PaymentDate { get; set; }
		public double Balance { get; set; }
		public string PaymentMethod { get; set; }
		public double AmountPaid { get; set; }
	
    }
}
