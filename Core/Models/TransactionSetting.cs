namespace Core.Models
{
    public class TransactionSetting : BaseModel
    {
        public string? KeyWord { get; set; }
        public string? ValidMessageChecker { get; set; }
    }
}
