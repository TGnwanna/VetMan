using Core.Models;
using static Core.Enums.VetManEnums;

namespace Core.ViewModels
{
    public class VaccineSubscriptionViewModel
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public Guid CustomerId { get; set; }
       
        public  string? FullName { get; set; }

        public DateTime DateDelivered { get; set; }

        public bool SmsSubscribed { get; set; }

        public bool EmailSubscribed { get; set; }

        public SubscriptionStatus SubscriptionStatus { get; set; }
        public double? Balance { get; set; }
        public Guid? CompanyBranchId { get; set; }
       
        public virtual CompanyBranch? CompanyBranch { get; set; }
    }
}
