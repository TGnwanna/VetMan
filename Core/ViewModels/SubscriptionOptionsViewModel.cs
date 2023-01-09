using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class SubscriptionOptionsViewModel
    {
        public bool SmsSubscribed { get; set; }
        public bool EmailSubscribed { get; set; }
        public string? TextArea { get; set; }
        public Guid? Id { get; set; }
    }
}
