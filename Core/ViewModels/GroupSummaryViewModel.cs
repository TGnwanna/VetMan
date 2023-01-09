using static Core.Enums.VetManEnums;

namespace Core.ViewModels
{
    public class GroupSummaryViewModel
    {
        public string GroupName { get; set; }
        public Guid? GroupId { get; set; }
        public Status Status { get; set; }
        public CustomerBookingViewModel Delivered { get; set; }
        public CustomerBookingViewModel Cancelled { get; set; }
        public CustomerBookingViewModel Booked { get; set; }
    }
}
