using static Core.Enums.VetManEnums;

namespace Core.ViewModels
{

    public class WalletHistoryViewModel
    {
        public Guid Id { get; set; }
        public int MyProperty { get; set; }
        public Guid? WalletId { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? CompanyBranchId { get; set; }
        public string? UserId { get; set; }
        public double Balance { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public string? Description { get; set; }
        public TransactionType TransactionType { get; set; }
        public string? CustomerEmail { get; set; }
    }

}