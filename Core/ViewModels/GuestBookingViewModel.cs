namespace Core.ViewModels
{
	public class GuestBookingViewModel
	{
		public Guid Id { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Email { get; set; }
		public string? PhoneNumber { get; set; }
		public string? Address { get; set; }
		public double Quantity { get; set; }
		public double TotalPrice { get; set; }
		public Guid? BookingId { get; set; }
		public Guid? CompanyBranchId { get; set; }
	}
}
