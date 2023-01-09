using Core.Models;
using Logic;
using Logic.IHelpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Logic.AppHttpContext;

namespace Vetman.Controllers
{
	[SessionTimeout]
	public class SubscriptionController : Controller
	{
		private readonly IUserHelper _userHelper;
		private readonly ISubscriptionHelper _subscription;

		public SubscriptionController(IUserHelper userHelper, ISubscriptionHelper subscription)
		{
			_userHelper = userHelper;
			_subscription = subscription;
		}


		public IActionResult Index()
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
			var subscriptions = _subscription.GetAllModule();
			return View(subscriptions);
		}

		public IActionResult Create(string ids)
		{
			if (ids != null)
			{
				var moduleIds = JsonConvert.DeserializeObject<List<Guid>>(ids);
				AppHttpContext.Current.Session.SetString("modules", ids);
                var url = _subscription.CreateSubscriber(moduleIds);
				if (url != null)
					return Json(new { isError = false, paystackUrl = url, msg = "You can proceed to payment" });

				return Json(new { isError = true, msg = "Subscription failed, try again" });
			}
			return Json(new { isError = true, msg = "Please select the module your want to subscribe to." });
		}
	}
}
