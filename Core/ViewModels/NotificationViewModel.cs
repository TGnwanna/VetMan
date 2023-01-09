namespace Core.ViewModels
{
	public class NotificationViewModel
	{
		public string Email { get; set; }
		public string PhoneNuber { get; set; }
		public string FirstName { get; set; }
		public double? NewPrice { get; set; }
		public bool IsDate { get; set; }
		public bool IsPrice { get; set; }
		public string? ExpectedDateOfArrival { get; set; }
	}
}
