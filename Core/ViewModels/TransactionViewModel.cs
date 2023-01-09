namespace Core.ViewModels
{
    public class TransactionViewModel
    {
        public Guid Id { get; set; }
        public string? ReceiptNo { get; set; }
        public string? Description { get; set; }
        public string? ConfirmDate { get; set; }
        public string? ConfirmedById { get; set; }
        public string? ConfirmedBy { get; set; }
    }
}
