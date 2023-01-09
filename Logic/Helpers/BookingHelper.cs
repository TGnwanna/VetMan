using AutoMapper;
using Core.Db;
using Core.Enums;
using Core.Models;
using Core.ViewModels;
using Logic.IHelpers;
using Logic.Services;
using Microsoft.EntityFrameworkCore;

namespace Logic.Helpers
{
	public class BookingHelper : IBookingHelper
	{
		private readonly AppDbContext _context;
		private readonly IAdminHelper _adminHelper;
		private readonly IMapper _mapper;
		private readonly IUserHelper _userHelper;

		public BookingHelper(AppDbContext context, IEmailService emailService, IMapper mapper, IEmailHelper emailHelper, IAdminHelper adminHelper, IUserHelper userHelper)
		{
			_context = context;
			_mapper = mapper;
			_adminHelper = adminHelper;
			_userHelper = userHelper;
		}
		public IEnumerable<CustomerBookingReadDto> GetAllBookings()
		{
			var bookings = new List<CustomerBookingReadDto>();
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

			}
			if (loggedInUser != null)
			{
				var accessRight = Utitily.GetRoleScreen();
				var bookinglist = _context.CustomerBookings.Where(x => x.CompanyBranchId == loggedInUser.CompanyBranchId && x.Status != VetManEnums.Status.Canceled)
					.Include(f => f.Customer).Include(z => z.BookingGroup).Include(z => z.BookingGroup.Product).AsQueryable();
				if (bookinglist.Count() == 0)
					return bookings;

				return _mapper.Map<IEnumerable<CustomerBookingReadDto>>(bookinglist);
			}
			return bookings;
		}

		public List<BookingGroupViewModel> GetBookingGroups()
		{
			var common = new BookingGroupViewModel()
			{
				Id = Guid.Parse("00000000-0000-0000-0000-000000000000"),
				Name = "-- Select --"
			};
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName)?.Result;

			var bookingGroup = _context.BookingGroups.Where(x => x.CompanyBranchId == loggedInUser.CompanyBranchId && x.Active).ToList();
			var groups = _mapper.Map<List<BookingGroupViewModel>>(bookingGroup);
			groups.Insert(0, common);
			return groups;
		}

        public List<BookingGroupViewModel> GetGuestBookingGroups(Guid id)
        {
            var common = new BookingGroupViewModel()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                Name = "-- Select --"
            };

            var bookingGroup = _context.BookingGroups.Where(x => x.CompanyBranchId == id && x.Active).ToList();
            var groups = _mapper.Map<List<BookingGroupViewModel>>(bookingGroup);
            groups.Insert(0, common);
            return groups;
        }
        public BookingGroupViewModel GetCurrentUserBookingGroup(Guid id)
		{
			if (id != Guid.Empty)
			{

				var bookingGroup = _context.BookingGroups.Where(x => x.Id == id && x.Active).FirstOrDefault();
				if (bookingGroup != null)
				{
					var groups = _mapper.Map<BookingGroupViewModel>(bookingGroup);
					return groups;
				}
			}
			return null;
		}

		public ClientInfoAndBookingViewModel RegisterClient(ClientInfoAndBookingViewModel customer)
		{
			var clientInfo = new ClientInfoAndBookingViewModel();
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

			}
			if (loggedInUser != null)
			{
				if (customer.FirstName == null)
				{
					customer.Error = "Please Enter Client First Name";
					return customer;
				}

				if (customer.PhoneNumber == null)
				{
					customer.Error = "Please the PhoneNumber field is required";
					return customer;
				}
				if (!customer.IsExistingClient)
				{
					customer.Active = true;
					customer.DateCreated = DateTime.Now;
					customer.CompanyBranchId = loggedInUser.CompanyBranchId;
					var client = _mapper.Map<Customer>(customer);
					_context.Customers.Add(client);
					_context.SaveChanges();
					customer.CustomerId = client.Id;
				}
				return customer;
			}
			return clientInfo;
		}

		public List<ClientInfoAndBookingViewModel> GetCustomers()
		{
			var common = new ClientInfoAndBookingViewModel()
			{
				Id = Guid.Parse("00000000-0000-0000-0000-000000000000"),
				FirstName = "-- Select --"
			};
			var loggedInUser = Utitily.GetCurrentUser();
			var allCustomer = _context.Customers.Where(x => x.Id != Guid.Empty && !x.IsDeleted && x.CompanyBranchId == loggedInUser.CompanyBranchId).ToList();
			var allClient = _mapper.Map<List<ClientInfoAndBookingViewModel>>(allCustomer).ToList();
			allClient.Insert(0, common);
			return allClient;

		}

		public string CreateClientBookings(ClientInfoAndBookingViewModel bookingDetails)
		{
			var customerBooking = _context.CustomerBookings.Where(x => x.BookingGroupId == bookingDetails.Id && x.CustomerId == bookingDetails.CustomerId).FirstOrDefault();
			if (customerBooking != null)
			{
				UpdateExistingCustomerBooking(bookingDetails, customerBooking);
				return "Successful";
			}
			CreateCustomerBookings(bookingDetails);
			return "Successful";
		}

		public bool CreateCustomerPayment(ClientInfoAndBookingViewModel bookingDetails, CustomerBooking booking)
		{
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

			}
			if (loggedInUser != null)
			{
				var customerPayment = new CustomerBookingPayment()
				{
					CustomerBookingId = booking.Id,
					DatePaid = DateTime.Now,
					Amount = bookingDetails.InitialAmount,
					UpdatedByUserId = bookingDetails.UpdatedByUserId,
					Quantity = bookingDetails.Quantity,
					CompanyBranchId = loggedInUser.CompanyBranchId,
					Active = true,

					DateCreated = DateTime.Now,
				};
				_context.CustomerBookingPayments.Add(customerPayment);
				_context.SaveChanges();
				return true;
			}
			return false;
		}

		public bool UpdateExistingCustomerBooking(ClientInfoAndBookingViewModel bookingDetails, CustomerBooking customerBooking)
		{
			var totalPrice = Convert.ToDouble(bookingDetails.Quantity) * Convert.ToDouble(bookingDetails.ProductPrice);
			var previousAmountPaidExistingClient = _context.CustomerBookingPayments.Where(x => x.CustomerBookingId == customerBooking.Id)?.Sum(f => f.Amount);
			var totalAmountOfShopedItems = customerBooking.Balance + previousAmountPaidExistingClient + totalPrice;
			var totalAmountPaidExistingClient = bookingDetails.InitialAmount + previousAmountPaidExistingClient;
			var balanceExistingClient = totalAmountOfShopedItems - totalAmountPaidExistingClient;
			customerBooking.Balance = balanceExistingClient;
			customerBooking.TotalPaid = totalAmountPaidExistingClient;
			customerBooking.CompanyBranchId = bookingDetails.CompanyBranchId;
			customerBooking.Status = VetManEnums.Status.Booked;
			customerBooking.QuantityBooked = bookingDetails.Quantity + customerBooking.QuantityBooked;
			_context.CustomerBookings.Update(customerBooking);
			_context.SaveChanges();

			CreateCustomerPayment(bookingDetails, customerBooking);
			return true;
		}


		public bool CreateCustomerBookings(ClientInfoAndBookingViewModel bookingDetails)
		{
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;
			}
			if (loggedInUser != null)
			{
				var totalPrice = Convert.ToDouble(bookingDetails.Quantity) * Convert.ToDouble(bookingDetails.ProductPrice);
				var balance = totalPrice - bookingDetails.InitialAmount;
				var booking = new CustomerBooking()
				{
					CustomerId = bookingDetails.CustomerId,
					BookingGroupId = bookingDetails.Id,
					QuantityBooked = bookingDetails.Quantity,
					Balance = balance,
					TotalPaid = bookingDetails.InitialAmount,
					Status = VetManEnums.Status.Booked,
					BookingDate = DateTime.Now,
					CompanyBranchId = loggedInUser.CompanyBranchId,
					Active = true,
					DateCreated = DateTime.Now,
				};
				_context.CustomerBookings.Add(booking);
				_context.SaveChanges();

				CreateCustomerPayment(bookingDetails, booking);
				return true;
			}
			return false;
		}

		public List<ProductType> GetProductTypeDropDown()
		{
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

			}
			var common = new ProductType()
			{
				Id = 0,
				Name = "-- Select --"
			};
			var productTypes = _context.ProductTypes.Where(x => !x.Deleted && x.CompanyBranchId == loggedInUser.CompanyBranchId).ToList();
			productTypes.Insert(0, common);
			return productTypes;
		}

		public CustomerBookingReadDto getBookingForEdit(Guid id)
		{
			var info = _context.CustomerBookings.Where(x => x.Id == id).Include(f => f.Customer).Include(z => z.BookingGroup).FirstOrDefault();
			return _mapper.Map<CustomerBookingReadDto>(info);
		}

		public bool EditClientBookings(CustomerBookingReadDto customerBooking)
		{
			var info = _context.CustomerBookings.Where(x => x.Id == customerBooking.Id)
				.Include(f => f.Customer).Include(z => z.BookingGroup).FirstOrDefault();
			if (info != null)
			{

				var currentAmountOfItem = Convert.ToDouble(customerBooking.QuantityBooked) * Convert.ToDouble(info.BookingGroup.ExpectedPrice);
				var previousAmountPaid = _context.CustomerBookingPayments.Where(x => x.CustomerBookingId == info.Id)?.Sum(f => f.Amount);
				var previousAmountOfitem = Convert.ToDouble(info.QuantityBooked) * Convert.ToDouble(info.BookingGroup.ExpectedPrice);

				var balance = currentAmountOfItem - previousAmountPaid;
				info.Balance = balance;
				info.QuantityBooked = customerBooking.QuantityBooked;
				_context.CustomerBookings.Update(info);
				_context.SaveChanges();

				UpdateCustomerBookingPayment(info);
				return true;
			}
			return false;
		}

		public bool UpdateCustomerBookingPayment(CustomerBooking customerBooking)
		{
			var customerPayment = _context.CustomerBookingPayments.Where(x => x.CustomerBookingId == customerBooking.Id && x.CompanyBranchId == customerBooking.CompanyBranchId).ToList();
			foreach (var item in customerPayment)
			{
				item.Quantity = Convert.ToInt32(customerBooking.QuantityBooked);
				_context.CustomerBookingPayments.Update(item);
			}

			_context.SaveChanges();

			//CreateCustomerPayment(bookingDetails, customerBooking);
			return true;
		}

		public List<BookingGroupViewModel> GetBookingByProductId(int productId)
		{
			var listOfBookings = new List<BookingGroupViewModel>();
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;
			}
			if (loggedInUser != null && productId != 0)
			{
				var bookingGroupList = _context.BookingGroups.Where(c => !c.Deleted && c.CompanyBranchId == loggedInUser.CompanyBranchId && c.ProductId == productId).Include(x => x.CompanyBranch)
				.Include(p => p.Product).Include(s => s.CustomerBookings)
				.Select(x => new BookingGroupViewModel
				{
					Name = x.Name,
					Id = x.Id,
					BranchId = x.CompanyBranchId,
					Product = x.Product.Name,
					ExpectedDateOfArrival = x.ExpectedDateOfArrival.Value.ToString("D"),
					ExpectedPrice = x.ExpectedPrice,
					ExpectedCostPrice = x.ExpectedCostPrice,
					TotalQuantityBooked = x.CustomerBookings.Sum(s => s.QuantityBooked),
					CustomerBookings = x.CustomerBookings,
					MotalityRecorded = x.MotalityRecorded,
					TotalBalance = x.CustomerBookings.Sum(x => x.Balance),
					TotalPaid = x.CustomerBookings.Sum(s => s.TotalPaid),
					SupplyPrice = x.SupplyPrice,

				}).ToList();
				if (bookingGroupList != null && bookingGroupList.Count > 0)
				{
					return bookingGroupList;
				}
			}
			return listOfBookings;
		}

		public List<BookingGroupViewModel> GetBookingGroupsByDateRange(DateTime startDate, DateTime endDate)
		{
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

			}
			var allBookingGroup = new List<BookingGroupViewModel>();
			var bookingGroups = _context.BookingGroups.Where(c => !c.Deleted && c.CompanyBranchId == loggedInUser.CompanyBranchId).Include(x => x.CompanyBranch)
				.Include(p => p.Product).Include(s => s.CustomerBookings).ToList();
			if (bookingGroups.Count > 0 && bookingGroups != null)
			{
				bookingGroups = bookingGroups.Where(x => !x.Deleted && x.ExpectedDateOfArrival >= startDate && x.ExpectedDateOfArrival >= endDate).ToList();
				var bookings = bookingGroups.Select(l => new BookingGroupViewModel
				{
					Name = l.Name,
					Id = l.Id,
					BranchId = l.CompanyBranchId,
					Product = l.Product.Name,
					ExpectedDateOfArrival = l.ExpectedDateOfArrival.Value.ToString("D"),
					ExpectedPrice = l.ExpectedPrice,
					ExpectedCostPrice = l.ExpectedCostPrice,
					TotalQuantityBooked = l.CustomerBookings.Sum(s => s.QuantityBooked),
					CustomerBookings = l.CustomerBookings,
					MotalityRecorded = l.MotalityRecorded,
					TotalBalance = l.CustomerBookings.Sum(x => x.Balance),
					TotalPaid = l.CustomerBookings.Sum(s => s.TotalPaid),
					SupplyPrice = l.SupplyPrice,
				}).ToList();
				if (bookings.Any())
				{
					return bookings;
				}
			}
			return allBookingGroup;
		}

		public GuestBooking AddBookingForGuest(GuestBookingViewModel guestBookingViewModel)
		{
			if (guestBookingViewModel != null)
			{
				var guestBooking = new GuestBooking()
				{
					FirstName = guestBookingViewModel.FirstName,
					LastName = guestBookingViewModel.LastName,
					Email = guestBookingViewModel.Email,
					PhoneNumber = guestBookingViewModel.PhoneNumber,
					Address = guestBookingViewModel.Address,
					CompanyBranchId = guestBookingViewModel.CompanyBranchId,
					BookingId = guestBookingViewModel.BookingId,
					Quantity = guestBookingViewModel.Quantity,
					TotalPrice = guestBookingViewModel.TotalPrice,
				};
				_context.Add(guestBooking);
				_context.SaveChanges();
				return guestBooking;
			}
			return null;
		}


	}
}
