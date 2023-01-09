namespace Core.ViewModels
{
    public class CompanySettingViewModel
    {
        public Guid? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public bool DOCBookingModule { get; set; }
        public bool VaccineModule { get; set; }
        public bool TransactionModule { get; set; }
        public bool StoreModule { get; set; }
    }
}
