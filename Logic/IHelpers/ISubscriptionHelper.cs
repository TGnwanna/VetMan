using Core.Models;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.IHelpers
{
	public interface ISubscriptionHelper
	{
        ModulesData GetAllModule();
		string CreateSubscriber(List<Guid> moduleIds);
		bool GetPaymentResponse(PaystackSubscription paystack);
		string GetEnumDescription(Enum value);
    }
}
