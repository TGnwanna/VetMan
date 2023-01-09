using Core.Models;

namespace Core.ViewModels
{
	public class CustomerPaymentGeneralViewModel
	{
		public List<CustomerBookingPaymentViewModel> CustomerBookingPaymentViewModel { get; set; }
		public List<CustomerBookingViewModel> CustomerBookingViewModel { get; set; }
		public CustomerBookingViewModel? CustomerId { get; set; }
		public string GroupName { get; set; }
		public string CustomerFullName { get; set; }
		public Guid? GroupId { get; set; }
		public string UpdatedByUserId { get; set; }
		public Guid CustomerBookingId { get; set; }
        public int CustomerCount { get; set; }
        public double SmsCost { get; set; }
    }
}
