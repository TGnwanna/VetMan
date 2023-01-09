namespace Core.ViewModels
{
    public class CustomerBookingPaymentViewModel
    {
        public Guid Id { get; set; }
        public string DatePaid { get; set; }
        public double? Amount { get; set; }
        public Guid? CustomerBookingId { get; set; }
        public string? BookingGroupName { get; set; }
        public string? UpdatedByUserId { get; set; }
        public string? UpdatedByUserName { get; set; }
        public string? CustomersFirstName { get; set; }
        public string? CustomersLastName { get; set; }
        public string? CustomersEmail { get; set; }
        public string? CustomersAddress { get; set; }
        public string Status { get; set; }
        public string? CustomersPhoneNumber { get; set; }
        public string? CustomersFullName { get; set; }
        public Guid? CustomersId { get; set; }

    }
}
