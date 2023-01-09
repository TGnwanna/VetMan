using System.ComponentModel.DataAnnotations.Schema;
using static Core.Enums.VetManEnums;

namespace Core.Models
{
    public class WalletHistory : BaseModel
    {
        public new Guid Id { get; set; }
        public int MyProperty { get; set; }
        public Guid? WalletId { get; set; }
        [ForeignKey("WalletId")]
        public virtual Wallet? Wallet { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public string? Description { get; set; }
        public TransactionType TransactionType { get; set; }
        public PayStackResponses PayStackResponses { get; set; }
    }
}
