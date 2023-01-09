using Core.Db;
using Logic.IHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Logic.AppHttpContext;

namespace Vetman.Controllers
{
	[SessionTimeout]
	[Authorize]//(Roles = "CompanyAdmin, SuperAdmin")]
	public class CompanyController : Controller
	{
		private readonly IAdminHelper _adminHelper;
		private readonly IUserHelper _userHelper;
		IEmailHelper _emailHelper;
		private readonly AppDbContext _context;
		private readonly ICustomerPaymentHelper _customerPaymentHelper;

		public CompanyController(AppDbContext context, IUserHelper userHelper, IAdminHelper adminHelper, IEmailHelper emailHelper, ICustomerPaymentHelper customerPaymentHelper)
		{
			_context = context;
			_emailHelper = emailHelper;
			_userHelper = userHelper;
			_adminHelper = adminHelper;
			_customerPaymentHelper = customerPaymentHelper;
		}

		[HttpGet]
		public IActionResult Index()
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
			return View();
		}
	}
}
