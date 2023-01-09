using Core.Db;
using Core.ViewModels;
using Logic.IHelpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Vetman.Controllers
{
    public class SalesPaymentController : Controller
    {
		private readonly IAdminHelper _adminHelper;
		private readonly IUserHelper _userHelper;
		private readonly IBookingHelper _bookingHelper;
		private readonly AppDbContext _context;
		private readonly IDropdownHelper _dropdownHelper;


		public SalesPaymentController(AppDbContext context, IUserHelper userHelper, IAdminHelper adminHelper, IBookingHelper bookingHelper, IDropdownHelper dropdownHelper)
		{
			_context = context;
			_userHelper = userHelper;
			_adminHelper = adminHelper;
			_bookingHelper = bookingHelper;
			_dropdownHelper = dropdownHelper;
		}


		public IActionResult Index(string id )
		{
			var salesPayment = new SalesPaymentViewModel();

			if (id != null && id != "")
            {
				var ide = id.Split("&");
				var idss = Convert.ToInt32(ide[0]);
				var amount = Convert.ToDouble(ide[1]);

				ViewBag.Layout = _userHelper.GetRoleLayout();
				ViewBag.GetBank = _userHelper.GetBank();
				
				if (amount != null && idss != 0)
				{
					var random = new Random();
					salesPayment.OrderId = random.Next(1000, 1000000);
					salesPayment.Banks = _userHelper.GetActiveBanks();
					salesPayment.SalesLogsId = idss;
					salesPayment.Amount = amount;
				}
			}
			
			return View(salesPayment);
		}


		[HttpPost]
		public JsonResult CreateBankPayment(string bankDetails)
		{
			if (bankDetails != null)
			{
				var commonDropDownViewModel = JsonConvert.DeserializeObject<CommonDropDowns>(bankDetails);
				if (commonDropDownViewModel != null)
				{

					var createBank = _userHelper.CreateBankDetails(commonDropDownViewModel);
					if (createBank)
					{
                        var id = commonDropDownViewModel.SalesLogId + "&" + commonDropDownViewModel.Amount;
                        return Json(new { isError = false, msg = "Bank Details Created Successfully", data = id });
					}
				}
				return Json(new { isError = true, msg = "Unable to Create Bank Details" });
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}
		[HttpPost]
		public JsonResult DeleteBank(string bankId)
		{
			var bankData = JsonConvert.DeserializeObject<CommonDropDowns>(bankId);
			if (bankData != null)
			{
				var bankDetails = _userHelper.DeleteBankDetails(bankData);
				if (bankDetails != null)
				{
					return Json(new { isError = false, msg = "Bank Deleted Successfully." });
				}
			}
			return Json(new { isError = true, msg = "Something Went Wrong." });
		}
		[HttpPost]
		public JsonResult CreatePaymentMeans(string paymentDetails)
		{
			if (paymentDetails != null)
			{
				var paymentMeansViewModel = JsonConvert.DeserializeObject<PaymentMeansViewModel>(paymentDetails);
				if (paymentMeansViewModel != null)
				{

					var createMeans = _userHelper.CreateBankPaymentMeansDetails(paymentMeansViewModel);
					if (createMeans)
					{
                        var id = paymentMeansViewModel.SalesLogId + "&" + paymentMeansViewModel.Amount;
                        return Json(new { isError = false, msg = "Payment Means Created Successfully", data = id });
					}
				}
				return Json(new { isError = true, msg = "Unable to Create Payment Means Details" });
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}
		[HttpPost]
		public JsonResult CreatePayment(string paymentDetails)
		{
			if (paymentDetails != null)
			{
				var salesPaymentViewModel = JsonConvert.DeserializeObject<SalesPaymentViewModel>(paymentDetails);
				if (salesPaymentViewModel != null)
				{
					var createPayment = _userHelper.CreatePaymentDetails(salesPaymentViewModel);
					if (createPayment)
					{
						return Json(new { isError = false, msg = " Sales Payment Created Successfully" });
					}
				}
				return Json(new { isError = true, msg = "Unable to Create Sales payment" });
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}

		public IActionResult GetPaymentMeans(int id )
		{
			if (id != 0) 
			{ 
				var availableMeans  = _userHelper.GetPayementMeans(id);
                return Json(new { isError = false, msg = " Sales Payment Created Successfully", data = availableMeans });
			}
				return Json(new { isError = true, msg = "Unable to Create Sales payment" });
		}

    }
}
