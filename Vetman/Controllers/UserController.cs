using Core.Db;
using Logic.IHelpers;
using Microsoft.AspNetCore.Mvc;
using static Logic.AppHttpContext;

namespace Vetman.Controllers
{
	[SessionTimeout]
	public class UserController : Controller
	{

		private AppDbContext _context;
		private IAdminHelper _adminHelper;
		private readonly IWalletHelper _walletHelper;
		private IUserHelper _userHelper;

		public UserController(AppDbContext context, IUserHelper userHelper, IAdminHelper adminHelper, IWalletHelper walletHelper)
		{
			_context = context;
			_adminHelper = adminHelper;
			_walletHelper = walletHelper;
			_userHelper = userHelper;
		}

		public IActionResult Index()
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Customer()
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
			var myCustomers = _adminHelper.GetCustomerList();
			if (myCustomers != null && myCustomers.Count > 0)
			{
				return View(myCustomers);
			}
			return View(myCustomers);
		}
	}
}
