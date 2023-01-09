using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Enums.VetManEnums;
using static Core.Models.PaystackResponse;

namespace Core.Models
{
    public  class VaccineSubscription : BaseModel
    {
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

        public Guid CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        public DateTime DateDelivered { get; set; }

        public bool SmsSubscribed { get; set; }

        public bool EmailSubscribed { get; set; }

        public SubscriptionStatus SubscriptionStatus { get; set; }
        
    }
}
