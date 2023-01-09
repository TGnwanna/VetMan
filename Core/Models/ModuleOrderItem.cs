using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ModuleOrderItem
    {
        public Guid Id { get; set; }
        public Guid PaystackSubscriptionId { get; set; }
        [ForeignKey("PaystackSubscriptionId")]
        public virtual PaystackSubscription? Subscription { get; set; }

        public Guid ModuleCostId { get; set; }
        [ForeignKey("ModuleCostId")]
        public virtual ModuleCost? ModuleCost { get; set; }
    }
}
