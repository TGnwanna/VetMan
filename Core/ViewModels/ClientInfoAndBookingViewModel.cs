namespace Core.ViewModels
{
    public class ClientInfoAndBookingViewModel
    {
        public Guid CustomerId { get; set; }
        public Guid? CompanyBranchId { get; set; }
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsExistingClient { get; set; }
        public string FullName => FirstName + " " + LastName;
        public string? Error { get; set; }
        public string? Profession { get; set; }
        public int? GenderId { get; set; }
        public double ProductPrice { get; set; }
        public double InitialAmount { get; set; }
        public int Quantity { get; set; }
        public DateTime DatePaid { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UpdatedByUserId { get; set; }
    }
}
