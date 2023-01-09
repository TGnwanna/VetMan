using AutoMapper;
using Core.Db;
using Core.Models;
using Core.ViewModels;
using Logic.IHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static Logic.AppHttpContext;

namespace Vetman.Controllers
{
	[SessionTimeout]
	[Authorize]//(Roles = "CompanyAdmin,SuperAdmin")]
	public class AdminController : Controller
	{
		private readonly IAdminHelper _adminHelper;
		private readonly IUserHelper _userHelper;
		private readonly AppDbContext _context;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IDropdownHelper _dropdownHelper;
		private readonly IMapper _mapper;
		private readonly ICompanyHelper _companyHelper;
		private readonly IWalletHelper _walletHelper;

		public AdminController(AppDbContext context, IUserHelper userHelper, IAdminHelper adminHelper, SignInManager<ApplicationUser> signInManager,
			UserManager<ApplicationUser> userManager, IDropdownHelper dropdownHelper, IMapper mapper, ICompanyHelper companyHelper, IWalletHelper walletHelper)
		{
			_context = context;
			_userHelper = userHelper;
			_adminHelper = adminHelper;
			_signInManager = signInManager;
			_userManager = userManager;
			_dropdownHelper = dropdownHelper;
			_mapper = mapper;
			_companyHelper = companyHelper;
			_walletHelper = walletHelper;
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


		[HttpGet]
		public JsonResult GetCustomerByID(string customerID)
		{
			if (customerID != null)
			{
				var customerId = JsonConvert.DeserializeObject<Guid>(customerID);
				var customer = _context.Customers.Where(c => c.Id == customerId).FirstOrDefault();
				if (customer != null)
				{
					return Json(customer);
				}
			}
			return null;
		}

		[HttpGet]
		public IActionResult BookingGroup()
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
			ViewBag.Product = _dropdownHelper.GetProducts();
			var myBookingGroup = _adminHelper.GetBookingGroupList();
			if (myBookingGroup != null && myBookingGroup.Count > 0)
			{
				return View(myBookingGroup);
			}
			return View(myBookingGroup);
		}

		[HttpPost]
		public async Task<JsonResult> CustomerPostAction(string customerDetails)
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
			if (customerDetails != null)
			{

				var customerViewModel = JsonConvert.DeserializeObject<CustomerViewModel>(customerDetails);
				if (customerViewModel != null)
				{
					if (customerViewModel.Email != string.Empty && customerViewModel.Email != null)
					{
						var emailValidation = _adminHelper.EmailCheck(customerViewModel);
						if (emailValidation)
							return Json(new { isError = true, msg = "Email used by Existing customer" });
					}
					var phoneNumberValidation = _adminHelper.PhoneNumberCheck(customerViewModel);
					if (phoneNumberValidation)
						return Json(new { isError = true, msg = "PhoneNumber used by Existing customer" });

					var createCustomer = _adminHelper.CustomerPostServer(customerViewModel);
					if (createCustomer)
					{
						return Json(new { isError = false, msg = "Customer created successfully" });
					}
				}
				return Json(new { isError = true, msg = "Unable to create customer" });
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}


		public JsonResult EditCustomer(string customerDetails)
		{
			if (customerDetails != null)
			{
				var CustomerViewModel = JsonConvert.DeserializeObject<CustomerViewModel>(customerDetails);
				if (CustomerViewModel != null)
				{
					var createCustomer = _adminHelper.GetEditCustomer(CustomerViewModel);
					if (createCustomer)
					{
						return Json(new { isError = false, msg = "Customer Updated successfully" });
					}
				}
				return Json(new { isError = false, msg = "Unable to update Customer" });
			}
			return Json(new { isError = false, msg = "Error Occurred" });
		}

		public JsonResult DeleteCustomer(string customerDetails)
		{
			if (customerDetails != null)
			{
				var CustomerViewModel = JsonConvert.DeserializeObject<CustomerViewModel>(customerDetails);
				if (CustomerViewModel != null)
				{
					var createCustomer = _adminHelper.DeleteCustomer(CustomerViewModel);
					if (createCustomer)
					{
						return Json(new { isError = false, msg = "Customer Deleted successfully" });
					}
				}
				return Json(new { isError = false, msg = "Unable to Delete Customer" });
			}
			return Json(new { isError = false, msg = "Error Occurred" });
		}


		[HttpPost]
		public JsonResult BookingGroup(string bookingGroupDetails)
		{
			ViewBag.Layout = _userHelper.GetRoleLayout();
			if (bookingGroupDetails != null)
			{
				var BookingGroupViewModel = JsonConvert.DeserializeObject<BookingGroupViewModel>(bookingGroupDetails);
				if (BookingGroupViewModel != null)
				{
					var createBookingGroup = _adminHelper.CreateBookingGroup(BookingGroupViewModel);
					if (createBookingGroup)
					{
						return Json(new { isError = false, msg = "BookingGroup created successfully" });

					}
					return Json(new { isError = true, msg = "Group name already exist" });
				}
				return Json(new { isError = true, msg = "Unable to create BookingGroup" });
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}

		[HttpGet]
		public IActionResult Index()
		{
			try
			{
				var loggedInUser = Utitily.GetCurrentUser();
				if (loggedInUser.Id == null)
				{
					loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

				}
				if (loggedInUser.UserRole != Utitily.Constants.CompanyStaffRole)
				{
					ViewBag.Layout = _userHelper.GetRoleLayout();
					var customers = _adminHelper.GetCustomerList();
					var customerCount = customers.Count();
					var selectedCustomer = customers.Take(5);
					var myCustomers = _mapper.Map<List<CustomerViewModel>>(selectedCustomer);

					var bookingGroup = _adminHelper.GetBookingGroupList();
					var bookingGroupCount = _context.BookingGroups.Where(c => !c.Deleted && c.CompanyBranchId == loggedInUser.CompanyBranchId).Count();

					var wallets = _walletHelper.GetCompanyTransactions();
					var branches = _companyHelper.GetCompanyBranchList();
					var staffsList = _userHelper.GetStaffList();
					var getCompanySubInfo = _userHelper.ModulesInfo();
					var adminDashBoard = new AdminDashboardViewModel();
                    if (getCompanySubInfo != null)
                    {

                        adminDashBoard = new AdminDashboardViewModel()
                        {
                            CustomerCount = customerCount,
                            Wallets = wallets,
                            BookingGroupCount = bookingGroupCount,
                            Customers = myCustomers,
                            BookingGroup = bookingGroup,
                            Branches = branches,
                            Staff = staffsList,
                            ExpiryDate = getCompanySubInfo.ExpiryDate,
                            CompanyId = getCompanySubInfo.CompanyId,
                            ModuleId = getCompanySubInfo.ModuleId,
                            SubcriptionStatus = getCompanySubInfo.SubcriptionStatus,

                        };
					}
					else
					{
                        adminDashBoard = new AdminDashboardViewModel()
                        {
                            CustomerCount = customerCount,
                            Wallets = wallets,
                            BookingGroupCount = bookingGroupCount,
                            Customers = myCustomers,
                            BookingGroup = bookingGroup,
                            Branches = branches,
                            Staff = staffsList,
                        };
                    }				
					return View(adminDashBoard);
				}
				return RedirectToAction("CantAccess");
			}
			catch (Exception exp)
			{

				throw exp;
			}

		}

		[HttpPost]
		public JsonResult EditBookingGroup(string bookingGroupDetails)
		{
			if (bookingGroupDetails != null)
			{
				var BookingGroupViewModel = JsonConvert.DeserializeObject<BookingGroupViewModel>(bookingGroupDetails);
				if (BookingGroupViewModel != null)
				{
					var createBookingGroup = _adminHelper.EditBookingGroup(BookingGroupViewModel);
					if (createBookingGroup)
					{
						return Json(new { isError = false, msg = "BookingGroup Updated successfully" });
					}
				}
				return Json(new { isError = false, msg = "Unable to update BookingGroup" });
			}
			return Json(new { isError = false, msg = "Error Occurred" });
		}

		[HttpPost]
		public JsonResult DeleteBookingGroup(string bookingGroupDetails)
		{
			if (bookingGroupDetails != null)
			{
				var BookingGroupViewModel = JsonConvert.DeserializeObject<BookingGroupViewModel>(bookingGroupDetails);
				if (BookingGroupViewModel != null)
				{
					var createBookingGroup = _adminHelper.DeleteBookingGroup(BookingGroupViewModel);
					if (createBookingGroup)
					{
						return Json(new { isError = false, msg = "BookingGroup Deleted successfully" });

					}
				}
				return Json(new { isError = false, msg = "Unable to Delete BookingGroup" });
			}
			return Json(new { isError = false, msg = "Error Occurred" });
		}


		[HttpGet]
		public JsonResult GetBookingGroupByID(string BookingGroupID)
		{
			if (BookingGroupID != null)
			{
				var BookingGroupId = JsonConvert.DeserializeObject<Guid>(BookingGroupID);
				var BookingGroup = _context.BookingGroups.Where(c => c.Id == BookingGroupId).Include(x => x.Product).FirstOrDefault();
				if (BookingGroup != null)
				{
					return Json(BookingGroup);
				}
			}
			return null;

		}


		[HttpGet]
		public JsonResult ArrivalReceipt(Guid groupId)
		{
			if (groupId != Guid.Empty)
			{
				ViewBag.Layout = _userHelper.GetRoleLayout();
				var bookingGroup = _context.BookingGroups.Where(x => x.Id == groupId && !x.Deleted).FirstOrDefault();
				if (bookingGroup != null)
				{
					var supplyDate = bookingGroup.SupplyDate?.Date.ToString("D");
					return Json(new { isError = false, data = bookingGroup, supplyDate = supplyDate });
				}
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}

		[HttpPost]
		public JsonResult ArrivalReceipt(string details)
		{
			if (details != null)
			{
				var bookingGroupViewModel = JsonConvert.DeserializeObject<BookingGroupViewModel>(details);
				if (bookingGroupViewModel != null)
				{
					bookingGroupViewModel.Id = Guid.Parse(bookingGroupViewModel.Code);
					var updateGroupDetails = _adminHelper.UpdateArrivalReceipt(bookingGroupViewModel);
					if (updateGroupDetails)
					{
						return Json(new { isError = false, data = bookingGroupViewModel.Id, msg = "Arrival Receipt Update successfully" });

					}
				}
				return Json(new { isError = false, msg = "Unable to Update ArrivalReceipt" });
			}
			return Json(new { isError = false, msg = "Error Occurred" });
		}



		[Authorize]
		public IActionResult Permission()
		{
			try
			{
				var loggedInUser = Utitily.GetCurrentUser();
				if (loggedInUser.Id == null)
				{
					loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

				}
				if ((loggedInUser.UserRole != Utitily.Constants.CompanyStaffRole) && (loggedInUser.UserRole != Utitily.Constants.CompanyManagerRole))
				{
					ViewBag.Layout = _userHelper.GetRoleLayout();
					var roles = _adminHelper.GetAvailableRoles();
					if (roles.Count > 0)
						return View(roles);
					return View();
				}
				return RedirectToAction("CantAccess");
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}
		[HttpGet]
		public IActionResult CantAccess()
		{
			return View();
		}

		public IActionResult CompanySetting(string id)
		{
			try
			{
				var loggedInUser = Utitily.GetCurrentUser();
				if (loggedInUser.Id == null)
				{
					loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

				}
				if (loggedInUser.UserRole != Utitily.Constants.CompanyStaffRole)
				{
					ViewBag.Layout = _userHelper.GetRoleLayout();
					var roleSettings = _adminHelper.GetAllScreensFromEnum(id);
					if (roleSettings != null)
					{
						roleSettings.Id = id;
						return View(roleSettings);
					}
					return View();
				}
				return RedirectToAction("CantAccess");
			}
			catch (Exception exp)
			{

				throw exp;
			}

		}

		public IActionResult UpdateAccess(string details, List<string> checkedRoleSettings, List<string> uncheckedRoleSettings)
		{
			try
			{
				var loggedInUser = Utitily.GetCurrentUser();
				if (loggedInUser.Id == null)
				{
					loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

				}
				if (loggedInUser.UserRole != Utitily.Constants.CompanyStaffRole)
				{
					if (details != null && (checkedRoleSettings.Count > 0 || uncheckedRoleSettings.Count > 0))
					{
						var branchAndRoleIds = JsonConvert.DeserializeObject<StaffViewModel>(details);
						var result = _adminHelper.UpdateRoleAccess(branchAndRoleIds, checkedRoleSettings, uncheckedRoleSettings);
						if (result != null)
							return Json(new { isError = false, msg = "Successful", data = result });
					}
					return Json(new { isError = true, msg = "Error" });
				}
				return RedirectToAction("CantAccess");
			}
			catch (Exception exp)
			{

				throw exp;
			}

		}
		public async Task<IActionResult> LogSuperAdminOut()
		{
			var currentAdmin = Utitily.GetCurrentUser();
			if (currentAdmin.Id == null)
			{
				currentAdmin = _userHelper.UpdateSessionAsync(currentAdmin.UserName).Result;

			}
			var impersonationRecords = _context.Impersonations.Where(x => x.CompanyAdminId == currentAdmin.Id).OrderBy(x => x.DateImpersonated)?.LastOrDefault();
			if (impersonationRecords != null)
			{
				impersonationRecords.EndSession = true;
				impersonationRecords.DateSessionEnded = DateTime.Now;
				_context.Update(impersonationRecords);
				_context.SaveChanges();

				var superAdmin = _userHelper.FindById(impersonationRecords.SuperAdminUserId);
				await _signInManager.SignOutAsync();
				HttpContext.Session.Clear();
				await _signInManager.SignInAsync(superAdmin, isPersistent: true);
				if (superAdmin.CompanyBranch?.Company != null)
					superAdmin.CompanyBranch.Company.CreatedBy = null;
				superAdmin.Roles = (List<string>)await _userManager.GetRolesAsync(superAdmin).ConfigureAwait(false);
				superAdmin.UserRole = Utitily.Constants.SuperAdminRole;
				var currentUser = JsonConvert.SerializeObject(superAdmin);
				HttpContext.Session.SetString("myuser", currentUser);
				return RedirectToAction("Index", "SuperAdmin");
			}
			else
			{
				await _signInManager.SignOutAsync();
				return RedirectToAction("Index", "SuperAdmin");
			}
		}


		[AllowAnonymous]
		public IActionResult Roles()
		{
			try
			{
				var loggedInUser = Utitily.GetCurrentUser();
				if (loggedInUser.Id == null)
				{
					loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

				}
				if ((loggedInUser.UserRole != Utitily.Constants.CompanyStaffRole) && (loggedInUser.UserRole != Utitily.Constants.CompanyManagerRole))
				{
					ViewBag.Layout = _userHelper.GetRoleLayout();
					ViewBag.Staff = _dropdownHelper.GetAllStaff();
					var roles = _context.Roles.Where(x => x.Name != Utitily.Constants.SuperAdminRole && x.Name != Utitily.Constants.CompanyAdminRole).ToList();
					ViewBag.Roles = _mapper.Map<List<RoleViewModel>>(roles);
					var role = _adminHelper.GetStaffRoles();
					return View(role);
				}
				return RedirectToAction("CantAccess");
			}
			catch (Exception)
			{

				throw;
			}

		}

		[HttpPost]
		public async Task<JsonResult> AddUserRole(string roleViewModel)
		{
			ViewBag.roles = _context.Roles.ToList();
			ViewBag.Layout = _userHelper.GetRoleLayout();

			var roles = JsonConvert.DeserializeObject<RoleViewModel>(roleViewModel);
			if (roles.StaffId != null)
			{
				var userDetails = _userHelper.FindById(roles?.StaffId);
				if (userDetails != null)
				{
					var getUserRole = await _userManager.GetRolesAsync(userDetails).ConfigureAwait(false);
					foreach (var role in getUserRole)
					{
						var removeFromRole = _userManager.RemoveFromRoleAsync(userDetails, role).Result;
					}
					var getRoleName = _context.Roles.Where(c => c.Id == roles.RoleId).FirstOrDefault();
					if (getRoleName != null)
					{
						var addToRole = _userManager.AddToRoleAsync(userDetails, getRoleName.Name).Result;
						if (addToRole != null)
						{
							return Json(new { isError = false, msg = "Role assigned successfully" });
						}
						return Json(new { isError = false, msg = "Unable to assign Role" });
					}
				}
			}

			return Json(new { isError = true, msg = "Unable to add Role" });
		}

		[HttpPost]
		public JsonResult ImpersonateCompanyBranch(Guid companyBranchId)
		{
			if (companyBranchId != Guid.Empty)
			{
				var companyBranch = _context.CompanyBranches.Where(x => x.Id == companyBranchId && x.Active).FirstOrDefault();
				if (companyBranch != null)
				{
					var user = _adminHelper.ImpersonateCompanyBranch(companyBranchId);
					if (user != null)
					{
						HttpContext.Session.SetString("myuser", user);
						return Json(new { isError = false, msg = "Branch Impersonated Successfully" });
					}
				}
				return Json(new { isError = true, msg = "Impersonated Failed" });
			}
			return Json(new { isError = true, msg = "Error Occurred" });
		}

		public IActionResult GetAllBooking()
		{
			var myBookingGroup = _adminHelper.GetAllBookingGroup();
			if (myBookingGroup.Count > 0)
			{
				return Json(new { isError = false, data = myBookingGroup });
			}
			return Json(new { isError = true, msg="No previous booking yet" });
		}

		public JsonResult GetBookingLink()
		{
			try
			{
				var currentUser = Utitily.GetCurrentUser();
				if (currentUser != null)
				{
					var companyBranchId = currentUser.CompanyBranchId;
					string linkToClick = HttpContext.Request.Scheme.ToString() + "://" +
					HttpContext.Request.Host.ToString() + "/Guest/GuestBooking/" + companyBranchId;
					return Json(linkToClick);
				}
				return null;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[HttpGet]
		public IActionResult ProductLogView(int id)
		{
			try
			{
				ViewBag.Layout = _userHelper.GetRoleLayout();
				ViewBag.productInventoryid = id;
				var ProducLogtList = _adminHelper.GetProductLogView(id);
				if (ProducLogtList != null && ProducLogtList.Count > 0)
				{
					return View(ProducLogtList);
				}
				return View(ProducLogtList);
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}
	}
}
