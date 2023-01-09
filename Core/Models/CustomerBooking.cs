using System.ComponentModel.DataAnnotations.Schema;
using static Core.Enums.VetManEnums;

namespace Core.Models
{
    public class CustomerBooking : BaseModel
    {
        public new Guid Id { get; set; }
        public Guid? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }
        public Guid? BookingGroupId { get; set; }
        [ForeignKey("BookingGroupId")]
        public virtual BookingGroup? BookingGroup { get; set; }
        public DateTime BookingDate { get; set; }
        public double? QuantityBooked { get; set; }
        public double? TotalPaid { get; set; }
        public double? Balance { get; set; }
        public Status Status { get; set; }
        //public bool IsGuest { get; set; }
    }
}
