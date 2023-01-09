namespace Core.ViewModels
{
    public class CustomerBookingReadDto
    {
        public Guid Id { get; set; }
        public string? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? BookingGroupName { get; set; }
        public Guid? BookingGroupId { get; set; }
        public DateTime? BookingDate { get; set; }
        public int QuantityBooked { get; set; }
        public double? TotalPaid { get; set; }
        public double? TotalAmount { get; set; }
        public double? Balance { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ExpectedPrice { get; set; }
        public string? ProductName { get; set; }
        public string? UpdatedByUserId { get; set; }
        public Guid? CompanyBranchId { get; set; }
        public DateTime DatePaid { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
