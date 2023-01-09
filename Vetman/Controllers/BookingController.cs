using Core.Db;
using Core.Enums;
using Core.ViewModels;
using Logic.IHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Logic.AppHttpContext;

namespace Vetman.Controllers
{
	[SessionTimeout]
	[Authorize]
	public class BookingController : Controller
	{
		private readonly IAdminHelper _adminHelper;
		private readonly IUserHelper _userHelper;
		private readonly IBookingHelper _bookingHelper;
		private readonly AppDbContext _context;
		private readonly IDropdownHelper _dropdownHelper;


		public BookingController(AppDbContext context, IUserHelper userHelper, IAdminHelper adminHelper, IBookingHelper bookingHelper, IDropdownHelper dropdownHelper)
		{
			_context = context;
			_userHelper = userHelper;
			_adminHelper = adminHelper;
			_bookingHelper = bookingHelper;
			_dropdownHelper = dropdownHelper;
		}
		[HttpGet]
		public IActionResult Index()
		{

			ViewBag.Layout = _userHelper.GetRoleLayout();
			var bookings = _bookingHelper.GetAllBookings().ToList();
			if (bookings != null && bookings.Count > 0)
			{
				return View(bookings);
			}
			return View(bookings);
		}

		[HttpGet]
		public IActionResult BookProduct()
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
			ViewBag.BookingGroup = _bookingHelper.GetBookingGroups();
			ViewBag.Customers = _bookingHelper.GetCustomers();

			return View();
		}

		public IActionResult UserBookings(string clientInfoAndBookings)
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
			if (clientInfoAndBookings != string.Empty)
			{
				var data = JsonConvert.DeserializeObject<ClientInfoAndBookingViewModel>(clientInfoAndBookings);
				if (data != null && data.IsExistingClient)
				{
					if (data.CustomerId == Guid.Empty)
						return Json(new { isError = true, msg = "Customer is required" });
				}

				if (data.Id == Guid.Empty)
					return Json(new { isError = true, msg = "Booking Group is required" });

				if (data.Quantity == 0)
					return Json(new { isError = true, msg = "Please enter quantity" });

				if (data.PhoneNumber == "")
					return Json(new { isError = true, msg = "Please enter phone Number" });

				if (data != null)
				{
					var loginUser = Utitily.GetCurrentUser();
					if (loginUser.Id == null)
					{
						loginUser = _userHelper.UpdateSessionAsync(loginUser.UserName).Result;

					}
					data.UpdatedByUserId = loginUser.Id;
					data.CompanyBranchId = loginUser.CompanyBranchId;
					if (!data.IsExistingClient)
						_bookingHelper.RegisterClient(data);
					else
						data.PhoneNumber = _context.Customers.Find(data?.CustomerId)?.PhoneNumber;
					var bookingMsg = _bookingHelper.CreateClientBookings(data);
					return Json(new { isError = false, msg = bookingMsg });
				}
			}
			return Json(new { isError = true, msg = "Please fill the form correctly" });
		}

		public IActionResult GetGroupPrice(Guid id)
		{
			if (id != Guid.Empty)
			{
				var bookinGroup = _context.BookingGroups.Find(id);
				if (bookinGroup != null)
					return Json(new { isError = false, data = bookinGroup.ExpectedPrice });
				return Json(new { isError = true, data = 0 });
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}

		public IActionResult EditBooking(Guid id)
		{
			if (id != Guid.Empty)
			{
				var bookingInfo = _bookingHelper.getBookingForEdit(id);
				return Json(new { isError = false, data = bookingInfo });
			}
			return Json(new { isError = true, msg = "Invalid selection" });
		}

		public IActionResult SaveEditedBooking(string group)
		{
			if (group != string.Empty)
			{
				var customerGroup = JsonConvert.DeserializeObject<CustomerBookingReadDto>(group);

				if (customerGroup?.QuantityBooked == 0)
					return Json(new { isError = true, msg = "Please enter quantity" });

				if (customerGroup != null)
				{
					var loginUser = Utitily.GetCurrentUser();
					if (loginUser.Id == null)
					{
						loginUser = _userHelper.UpdateSessionAsync(loginUser.UserName).Result;
					}
					customerGroup.UpdatedByUserId = loginUser.Id;
					_bookingHelper.EditClientBookings(customerGroup);
					return Json(new { isError = false, msg = "Successful" });
				}
			}
			return Json(new { isError = true, msg = "Please fill the form correctly" });
		}

		public IActionResult DeleteCustomerBooking(Guid id)
		{
			if (id != Guid.Empty)
			{
				var customerData = _context.CustomerBookings.Find(id);
				if (customerData != null)
				{
					customerData.Status = VetManEnums.Status.Canceled;
					_context.CustomerBookings.Update(customerData);
					_context.SaveChanges();
					return Json(new { isError = false, msg = "Successful" });
				}
				return Json(new { isError = true, msg = "Failed" });
			}
			return Json(new { isError = true, msg = "Error occurred" });
		}

		[HttpGet]
		public IActionResult AddCustomer(Guid id)
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
			if (id != Guid.Empty)
			{
				var BookingGroupViewModel = _bookingHelper.GetCurrentUserBookingGroup(id);
				ViewBag.Customers = _bookingHelper.GetCustomers();
				return View(BookingGroupViewModel);
			}
			return View();
		}

		[HttpGet]
		public IActionResult ProductBookings(int projectId)
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
			ViewBag.Product = _dropdownHelper.GetProducts();
			var bookings = _bookingHelper.GetBookingByProductId(projectId);
			if (bookings != null && bookings.Count > 0)
			{
				return View(bookings);
			}
			return View(bookings);
		}
		[HttpPost]
		public JsonResult GetBookingsByDateRange(DateTime dateFrom, DateTime dateTo)
		{
			if (dateFrom != null && dateTo != null)
			{
				var bookingGroups = _bookingHelper.GetBookingGroupsByDateRange(dateFrom, dateTo);
				if (bookingGroups.Count > 0 && bookingGroups != null)
				{
					return Json(new { isError = false, data = bookingGroups });
				}
				return Json(new { isError = true, msg = "No data available for this date range" });
			}
			return Json(new { isError = true, msg = "Please Enter details" });
		}
	}
}
