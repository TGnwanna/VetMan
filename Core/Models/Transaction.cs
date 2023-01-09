using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public string? ConfirmedById { get; set; }
        [ForeignKey("ConfirmedById")]
        public virtual ApplicationUser? ConfirmedBy { get; set; }
        public string? ReceiptNo { get; set; }
        public DateTime? ConfirmDate { get; set; }
        public bool IsConfirmed { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public string? Description { get; set; }
        public DateTime? DateCreated { get; set; }
        public Guid? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company? Company { get; set; }
    }
}
