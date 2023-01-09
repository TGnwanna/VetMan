using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class CustomerBookingPayment : BaseModel
    {
        public new Guid Id { get; set; }
        public DateTime DatePaid { get; set; }
        public double? Amount { get; set; }
        public int Quantity { get; set; }
        public Guid? CustomerBookingId { get; set; }
        [ForeignKey("CustomerBookingId")]
        public virtual CustomerBooking? CustomerBooking { get; set; }
        public string? UpdatedByUserId { get; set; }
        [ForeignKey("UpdatedByUserId")]
        public virtual ApplicationUser? UpdatedBy { get; set; }

    }
}
