using Core.Enums;
using Core.Models;
using System.Collections.ObjectModel;

namespace Core.ViewModels
{
	public class BookingGroupViewModel
	{
		public Guid Id { get; set; }
		public string Code { get; set; }
		public string? Name { get; set; }
		public DateTime? ExpectedDateOfArrivalD { get; set; }
		public DateTime SupplyDateD { get; set; }
		public string? ExpectedDateOfArrival { get; set; }
		public double? ExpectedCostPrice { get; set; }
		public int? MotalityRecorded { get; set; }
		public int? QuantityLeft { get; set; }
		public int? QuantitySold { get; set; }
		public double? SupplyPrice { get; set; }
		public int? QuantityArrived { get; set; }
		public string SupplyDate { get; set; }
		public double? ExpectedPrice { get; set; }
		public int? ProductTypeId { get; set; }
		public int? ProductId { get; set; }
		public string? Product { get; set; }
		public string? ProductName { get; set; }
		public bool SendSMS { get; set; }
		public bool SendEmail { get; set; }
		public Guid? BranchId { get; set; }
		public GeneralActions ActionType { get; set; }

		public bool IsPrice { get; set; }
		public bool IsDate { get; set; }
		public double? TotalQuantityBooked { get; set; }
        public virtual Collection<CustomerBooking>? CustomerBookings { get; set; }

		public double? TotalPaid { get; set; }
		public double? TotalBalance { get; set; }
	}
}
