using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
	public class BookingGroup : BaseModel
	{
		public new Guid Id { get; set; }
		public DateTime? ExpectedDateOfArrival { get; set; }
		public DateTime? SupplyDate { get; set; }
		public double? ExpectedPrice { get; set; }
		public double? SupplyPrice { get; set; }
		public double? ExpectedCostPrice { get; set; }
		public int? MotalityRecorded { get; set; }
		public int? QuantityLeft { get; set; }
		public int? QuantitySold { get; set; }
		public int? QuantityArrived { get; set; }
		public int? ProductId { get; set; }
		[ForeignKey("ProductId")]
		public virtual Product? Product { get; set; }
		public virtual Collection<CustomerBooking>? CustomerBookings { get; set;}
	}
}
