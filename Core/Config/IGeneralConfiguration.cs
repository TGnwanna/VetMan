namespace Core.Config
{
    public interface IGeneralConfiguration
    {
        string AdminEmail { get; set; }
        public string BuckSMSBaseURL { get; set; }
        public string BuckSMSEmail { get; set; }
        public string BuckSMSPassword { get; set; }
        public string BuckSMSforcednd { get; set; }
        public string BULKSMSApiKey { get; set; }
        string PayStakApiKey { get; set; }
        double SmsAmount { get; set; }
        public string SmsNotificationTime { get; set; }
        string CompanyId { get; set; }
        string TimeToNotifyCompany { get; set; }
        string DeveloperEmail { get; set; }
    }
}
