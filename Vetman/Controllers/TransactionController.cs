using Core.Db;
using Core.ViewModels;
using Hangfire;
using Logic.IHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Logic.AppHttpContext;

namespace Vetman.Controllers
{
	[SessionTimeout]
	public class TransactionController : Controller
	{
		private readonly IAdminHelper _adminHelper;
		private readonly IUserHelper _userHelper;
		IEmailHelper _emailHelper;
		private readonly AppDbContext _context;
		private readonly ICompanyHelper _companyHelper;

		public TransactionController(AppDbContext context, IUserHelper userHelper, IAdminHelper adminHelper, IEmailHelper emailHelper, ICompanyHelper companyHelper)
		{
			_context = context;
			_emailHelper = emailHelper;
			_userHelper = userHelper;
			_adminHelper = adminHelper;
			_companyHelper = companyHelper;
		}

		[HttpGet]
		public IActionResult Index()
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
			var transactions = _companyHelper.AllTransactions();
			if (transactions != null && transactions.Count > 0)
			{
				return View(transactions);
			}
			return View(transactions);
		}
		[HttpPost]
		public IActionResult SaveIncomingMessage(string messageBody, string sender)
		{
			var checkForValidsender = _context.TransactionSettings.Where(x => x.Id != 0 && x.Name == sender && !x.Deleted).FirstOrDefault();
			if (checkForValidsender != null)
			{
				if (messageBody != null)
				{
					BackgroundJob.Enqueue(() => _companyHelper.SaveIncomingTrasactionMessage(messageBody, checkForValidsender.KeyWord, checkForValidsender.ValidMessageChecker));
					return Ok();
				}
			}
			return BadRequest();
		}


		[HttpPost]
		public JsonResult ConfirmTransaction(Guid transactionId, string paymentReceipt)
		{
			if (transactionId != Guid.Empty && paymentReceipt != null)
			{
				var confirmTransaction = _companyHelper.ConfirmTransaction(transactionId, paymentReceipt);
				if (confirmTransaction)
				{
					return Json(new { isError = false, mgs = "Transaction confirmed and updated" });
				}
				return Json(new { isError = true, mgs = "transaction not confirmed" });
			}
			return Json(new { isError = true, mgs = "Error occurred" });
		}

		[Authorize]
		[HttpGet]
		public IActionResult TransactionSettings()
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
			var transactionSettings = _companyHelper.TransactionSetting();
			if (transactionSettings != null && transactionSettings.Count > 0)
			{
				return View(transactionSettings);
			}
			return View(transactionSettings);
		}

		[Authorize]
		[HttpPost]
		public JsonResult TransactionSetUp(string setupDetails)
		{
			if (setupDetails != null)
			{
				var transactionModel = JsonConvert.DeserializeObject<TransactionSettingsViewModel>(setupDetails);
				if (transactionModel != null)
				{
					var setUpTransaction = _companyHelper.SetUpTransaction(transactionModel);
					if (setUpTransaction)
					{
						return Json(new { isError = false, msg = "Details Saved Successfully" });
					}
				}
			}
			return Json(new { isError = true, msg = "Unable to setup transaction" });
		}

		[Authorize]
		[HttpGet]
		public JsonResult EditTransactionSetUp(int transactionSettingId)
		{
			if (transactionSettingId != 0)
			{
				var transactionModel = _context.TransactionSettings.Where(x => x.Id == transactionSettingId && !x.Deleted).FirstOrDefault();
				if (transactionModel != null)
				{
					return Json(new { isError = false, data = transactionModel });
				}
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}

		[Authorize]
		[HttpPost]
		public JsonResult EditTransactionSetUp(string setupDetails)
		{
			if (setupDetails != null)
			{
				var transactionModel = JsonConvert.DeserializeObject<TransactionSettingsViewModel>(setupDetails);
				if (transactionModel != null)
				{
					var setUpTransaction = _companyHelper.EditTransactionSetup(transactionModel);
					if (setUpTransaction)
					{
						return Json(new { isError = false, msg = "Details edited Successfully" });
					}
				}
			}
			return Json(new { isError = true, msg = "Unable to edit transaction settings" });
		}

		[Authorize]
		[HttpPost]
		public JsonResult DeleteTransactionSetUp(int transactionSettingId)
		{
			if (transactionSettingId != 0)
			{
				var setUpTransaction = _companyHelper.DeleteTransactionSetUp(transactionSettingId);
				if (setUpTransaction)
				{
					return Json(new { isError = false, msg = "Details deleted Successfully" });
				}
			}
			return Json(new { isError = true, msg = "Unable to delete transaction settings" });
		}
	}
}
