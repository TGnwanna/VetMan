namespace Core.Config
{
    public class GeneralConfiguration : IGeneralConfiguration
    {
        public string AdminEmail { get; set; }
        public string BuckSMSBaseURL { get; set; }
        public string BuckSMSEmail { get; set; }
        public string BuckSMSPassword { get; set; }
        public string BuckSMSforcednd { get; set; }
        public string BULKSMSApiKey { get; set; }
        public string PayStakApiKey { get; set; }
        public double SmsAmount { get; set; }
        public string SmsNotificationTime { get; set; }
        public string CompanyId { get; set; }
        public string TimeToNotifyCompany { get; set; }
        public string DeveloperEmail { get; set; }
    }
}
