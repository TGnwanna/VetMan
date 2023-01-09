namespace Core.ViewModels
{
    public class CustomerBookingViewModel
    {
        public Guid Id { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? BookingGroupId { get; set; }
        public string? BookingDate { get; set; }
        public double? QuantityBooked { get; set; }
        public double? TotalPaid { get; set; }
        public double? Balance { get; set; }
        public string Status { get; set; }
        public int TotalDelivered { get; set; }
        public string? CustomersFirstName { get; set; }
        public double? ExpectedPrice { get; set; }
        public string? CustomersLastName { get; set; }
        public string? CustomersEmail { get; set; }
        public string? CustomersAddress { get; set; }
        public string? CustomersPhoneNumber { get; set; }
        public string? CustomersFullName { get; set; }
        public string? BookingGroupName { get; set; }
        public int TotalBooked { get; set; }
        public decimal TotalCancelled { get; set; }
		public double? ExpectedCostPrice { get; set; }
		public int? MotalityRecorded { get; set; }
		public int? QuantityLeft { get; set; }
		public int QuantitySold { get; set; }
		public int? SupplyPrice { get; set; }
		public double? ProfitMade { get; set; }
		public double? PotentialProfit { get; set; }
		public int? QuantityArrived { get; set; }
		
	}
}
