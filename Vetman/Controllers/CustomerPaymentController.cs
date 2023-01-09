using Core.Db;
using Core.ViewModels;
using Logic.IHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static Core.Enums.VetManEnums;
using static Logic.AppHttpContext;

namespace Vetman.Controllers
{
	[SessionTimeout]
	[Authorize]
	public class CustomerPaymentController : Controller
	{
		private readonly IAdminHelper _adminHelper;
		private readonly IUserHelper _userHelper;
		IEmailHelper _emailHelper;
		private readonly AppDbContext _context;
		private readonly ICustomerPaymentHelper _customerPaymentHelper;

		public CustomerPaymentController(AppDbContext context, IUserHelper userHelper, IAdminHelper adminHelper, IEmailHelper emailHelper, ICustomerPaymentHelper customerPaymentHelper)
		{
			_context = context;
			_emailHelper = emailHelper;
			_userHelper = userHelper;
			_adminHelper = adminHelper;
			_customerPaymentHelper = customerPaymentHelper;
		}

		[HttpGet]
		public IActionResult ManageGroup(Guid bookingGroupId)
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
			ViewBag.BookingGroup = _customerPaymentHelper.GetAllBookingGroup();
			var customers = _customerPaymentHelper.GetCustomersInGroup(bookingGroupId);
			if (customers != null && customers.CustomerBookingViewModel != null && customers.CustomerBookingViewModel.Any())
			{
				return View(customers);
			}
			return View(customers);
		}

		[HttpGet]
		public async Task<ActionResult> CustomerPayment(Guid customerBookingId)
		{
			var customers = new CustomerPaymentGeneralViewModel();
			ViewBag.Layout = _userHelper.GetRoleLayout();
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName)?.Result;

			if (loggedInUser != null)
			{
				customers = _customerPaymentHelper.GetCustomerBookingPayment(customerBookingId, loggedInUser);
				if (customers.CustomerBookingPaymentViewModel.Any())
				{
					return View(customers);
				}
			}
			return View(customers);

		}

		[HttpGet]
		public JsonResult UpdatePrice(Guid bookingGroupId)
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
			if (bookingGroupId != Guid.Empty)
			{
				var bookingGroup = _context.BookingGroups.Where(x => x.Id == bookingGroupId).FirstOrDefault();
				if (bookingGroup != null)
				{
					var date = bookingGroup.ExpectedDateOfArrival?.ToString("D");
					return Json(new { data = bookingGroup, date = date });
				}
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}

		[HttpGet]
		public JsonResult UpdateCustomerBookingDetails(Guid customerPaymentId)
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
			if (customerPaymentId != Guid.Empty)
			{
				var loggedInUser = Utitily.GetCurrentUser();
				if (loggedInUser.Id == null)
					loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName)?.Result;

				if (loggedInUser != null)
				{
					var customerBooking = _context.CustomerBookings.Where(x => x.Id == customerPaymentId && x.Active && x.CompanyBranchId == loggedInUser.CompanyBranchId).FirstOrDefault();
					if (customerBooking?.Balance <= 0)
					{
						var status = Status.Delivered.ToString();
						if (customerBooking.Status == Status.Delivered)
							return Json(new { isError = true, msg = "Product has already been Delivered", data = status });
						return Json(new { isError = false, data = customerBooking });
					}
					else
					{
						return Json(new { isError = true, msg = "Pay up the remaining balance for the booking status to change" });
					}
				}
			}
			return Json(new { isError = true, msg = " Error Occurred " });
		}

		[HttpPost]
		public JsonResult UpdatePrice(string priceDetails)
		{
			if (priceDetails != null)
			{
				var bookingGroupViewModel = JsonConvert.DeserializeObject<BookingGroupViewModel>(priceDetails);
				if (bookingGroupViewModel != null)
				{
					var updatePrice = _customerPaymentHelper.UpdatePrice(bookingGroupViewModel);
					if (updatePrice != null)
					{
						_emailHelper.GetCustomerAndSendMessage(bookingGroupViewModel);
						_customerPaymentHelper.UpdateGroupPaymenWithHangFire(updatePrice.Id);
						return Json(new { isError = false, msg = "Price Updated Successfully", data = updatePrice });
					}
				}
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}

		[HttpPost]
		public JsonResult AddPayment(string paymentDetails)
		{
			if (paymentDetails != null)
			{
				var customerBookingPaymentViewModel = JsonConvert.DeserializeObject<CustomerBookingPaymentViewModel>(paymentDetails);
				if (customerBookingPaymentViewModel != null)
				{
					var updatePrice = _customerPaymentHelper.AddPayment(customerBookingPaymentViewModel);
					if (updatePrice != null)
					{
						return Json(new { isError = false, msg = "Payment added Successfully", result = updatePrice.CustomerBookingId });
					}
				}
			}
			return Json(new { isError = true, msg = "Unable to add payment" });
		}

		[HttpPost]
		public JsonResult UpdateDate(string dateDetails)
		{
			if (dateDetails != null)
			{
				var bookingGroupViewModel = JsonConvert.DeserializeObject<BookingGroupViewModel>(dateDetails);
				if (bookingGroupViewModel != null)
				{
					var updatePrice = _customerPaymentHelper.UpdateDate(bookingGroupViewModel);
					if (updatePrice != null)
					{
						bookingGroupViewModel.BranchId = updatePrice.CompanyBranchId;
						_emailHelper.GetCustomerAndSendMessage(bookingGroupViewModel);
						return Json(new { isError = false, msg = "Date Updated Successfully", data = updatePrice });
					}
				}
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}

		[HttpGet]
		public IActionResult GroupSummary(Guid id)
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
			if (id != Guid.Empty)
			{
				var groupSummary = _customerPaymentHelper.GetGroupSummary(id);
				if (groupSummary != null)
				{
					return View(groupSummary);
				}
				return View(groupSummary);
			}
			return View();
		}

		[HttpPost]
		public JsonResult UpdateCustomerBookingStatus(string subscriberOption)
		{
			if (subscriberOption != null)
			{
				var details = JsonConvert.DeserializeObject<SubscriptionOptionsViewModel>(subscriberOption);
				if (details.Id != Guid.Empty)
				{
					var loggedInUser = Utitily.GetCurrentUser();
					if (loggedInUser.Id == null)
						loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName)?.Result;

					if (loggedInUser != null)
					{
						var customerBooking = _context.CustomerBookings.Where(x => x.Id == details.Id && x.Active && x.CompanyBranchId == loggedInUser.CompanyBranchId).FirstOrDefault();
						if (customerBooking != null)
						{
							if (customerBooking.Balance > 0)
							{
								return Json(new { isError = true, msg = "Pay up the remaining balance for the booking status to change", bookingGroupId = customerBooking.Id });
							}
						}
						var bookingGroupId = _customerPaymentHelper.UpdateCustomerBookingStatus(customerBooking, details);
						if (bookingGroupId != null)
						{
							return Json(new { isError = false, msg = "Status updated successfully", bookingGroupId = bookingGroupId });
						}
					}
				}
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}

		[HttpGet]
		public JsonResult MoveUserToNewGroup(Guid customerBookingId)
		{
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName)?.Result;

			if (loggedInUser != null)
			{
				if (customerBookingId != Guid.Empty)
				{
					var customeBooking = _context.CustomerBookings.Where(x => x.Id == customerBookingId && x.Active && x.CompanyBranchId == loggedInUser.CompanyBranchId).Include(x => x.BookingGroup)
						.Select(x => new CustomerBookingPaymentViewModel()
						{
							Id = x.Id,
							BookingGroupName = x.BookingGroup.Name,
							Status = x.Status.ToString(),
						})
						.FirstOrDefault();
					if (customeBooking != null)
					{
						var status = Status.Delivered.ToString();
						if (customeBooking.Status == status)
							return Json(new { isError = true, msg = "Delivered Customer can not be moved to a new group" });

						return Json(new { isError = false, data = customeBooking });
					}
				}
			}
			return Json(new { isError = true, msg = "Error occurred" });
		}

		[HttpPost]
		public JsonResult MoveUserToNewGroup(string groupDetails)
		{
			if (groupDetails != null)
			{
				var custormerDetails = JsonConvert.DeserializeObject<CustomerBookingViewModel>(groupDetails);
				if (custormerDetails != null)
				{
					var customerNewGroup = _customerPaymentHelper.MoveToNewGroup(custormerDetails);
					if (customerNewGroup != null)
					{
						return Json(new { isError = false, msg = "Customer moved successfully", bookingGroupId = customerNewGroup.BookingGroupId });
					}
					return Json(new { isError = false, msg = "Unable to move customer to group", bookingGroupId = customerNewGroup.BookingGroupId });
				}
			}
			return Json(new { isError = false, msg = "Error Occurred" });
		}

		[HttpPost]
		public JsonResult MoveCustomersToNewGroup(Guid bookingGroupId, Guid newGroupId)
		{
			if (bookingGroupId != Guid.Empty && newGroupId != Guid.Empty)
			{
				var loggedInUser = Utitily.GetCurrentUser();
				if (loggedInUser.Id == null)
					loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName)?.Result;

				if (loggedInUser != null)
				{
					var getBookedCustomers = _customerPaymentHelper.GetBookedCustomer(bookingGroupId, loggedInUser.CompanyBranchId);
					if (getBookedCustomers.Count() != 0)
					{
						_customerPaymentHelper.SyncCustmerPayment(bookingGroupId, newGroupId, loggedInUser.CompanyBranchId);
						return Json(new { isError = false, msg = "Customer moved successfully. Customer Payment update in progress", bookingGroupId = bookingGroupId });
					}
					return Json(new { isError = true, msg = "Delivered product can not be moved to a new group" });
				}
			}
			return Json(new { isError = true, msg = "Unable to move customer to group", bookingGroupId = bookingGroupId });
		}


	}
}
