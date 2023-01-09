using AutoMapper;
using Core.Config;
using Core.Db;
using Core.Models;
using Core.ViewModels;
using Logic.IHelpers;
using Logic.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel;
using static Core.Enums.VetManEnums;

namespace Logic.Helpers
{
	public class AdminHelper : IAdminHelper
	{
		private readonly IEmailHelper _emailHelper;
		private readonly IUserHelper _userHelper;
		private readonly AppDbContext _context;
		private readonly IEmailService _emailService;
		private readonly IMapper _mapper;
		private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGeneralConfiguration _generalConfiguration;

        public AdminHelper(AppDbContext context, IEmailService emailService, IMapper mapper, IEmailHelper emailHelper, UserManager<ApplicationUser> userManager, IUserHelper userHelper, IGeneralConfiguration generalConfiguration)
		{
			_context = context;
			_emailHelper = emailHelper;
			_emailService = emailService;
			_mapper = mapper;
			_userManager = userManager;
			_userHelper = userHelper;
			_generalConfiguration = generalConfiguration;
		}
		// Get Product
		public List<ProductViewModel> GetProductList()
		{
			var products = new List<ProductViewModel>();
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName)?.Result;

			if (loggedInUser != null)
			{
				products = _context.Products
				.Where(x => x.Id != 0 && !x.Deleted && x.CompanyBranchId == loggedInUser.CompanyBranchId)
				.Select(s => new ProductViewModel()
				{
					Id = s.Id,
					Name = s.Name,
					Active = s.Active,
					DateCreated = s.DateCreated,
				}).ToList();

			}
			return products;
		}
		//Get Product type
		public List<ProductTypeViewModel> GetProductTypeList()
		{
			var productTypeViewModel = new List<ProductTypeViewModel>();
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName)?.Result;

			if (loggedInUser != null)
			{
				var productType = _context.ProductTypes
			   .Where(x => x.Id != 0 && !x.Deleted && x.CompanyBranchId == loggedInUser.CompanyBranchId)
			   .Select(s => new ProductTypeViewModel()
			   {
				   Id = s.Id,
				   Name = s.Name,
				   Active = s.Active,
				   DateCreated = s.DateCreated
			   }).ToList();
				return productType;
			}
			return productTypeViewModel;
		}
		public bool CreateProductDetails(ProductViewModel productDetails)
		{
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName)?.Result;

			if (loggedInUser != null)
			{
				var productValidation = _context.Products.Where(c => c.Name == productDetails.Name && !c.Deleted && c.CompanyBranchId == loggedInUser.CompanyBranchId).FirstOrDefault();
				if (productValidation == null)
				{
					var productModel = new Product()
					{
						Name = productDetails.Name,
						DateCreated = DateTime.Now,
						ProductTypeId = productDetails.ProductTypeId,
						Active = true,
						Deleted = false,
						CompanyBranchId = loggedInUser.CompanyBranchId,
					};
					_context.Products.Add(productModel);
					_context.SaveChanges();
					return true;
				}
			}
			return false;
		}
		public bool CreateProductTypeDetails(ProductTypeViewModel productTypeDetails)
		{
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName)?.Result;

			if (loggedInUser != null)
			{
				var productTypeValidation = _context.ProductTypes.Where(c => c.Name == productTypeDetails.Name && c.CompanyBranchId == loggedInUser.CompanyBranchId).FirstOrDefault();
				if (productTypeValidation == null)
				{
					var productTypeModel = new ProductType();
					productTypeModel.CompanyBranchId = loggedInUser.CompanyBranchId;
					productTypeModel.Name = productTypeDetails.Name;
					productTypeModel.DateCreated = DateTime.Now;
					productTypeModel.Active = true;
					productTypeModel.Deleted = false;

					_context.ProductTypes.Add(productTypeModel);
					_context.SaveChanges();
					return true;
				}
			}
			return false;
		}
		public Product EditProductDetails(int productId)
		{
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName)?.Result;

			if (loggedInUser != null)
			{
				if (productId != 0)
				{
					var productToEdit = _context.Products.Where(x => x.Id == productId && x.Active && !x.Deleted && x.CompanyBranchId == loggedInUser.CompanyBranchId).Include(f => f.ProductType).FirstOrDefault();
					if (productToEdit != null)
						return productToEdit;
				}
			}
			return null;
		}
		public ProductType EditProductTypeDetails(int productTypeId)
		{
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName)?.Result;

			if (loggedInUser != null)
			{
				if (productTypeId != 0)
				{
					var productTypeToEdit = _context.ProductTypes.Where(x => x.Id == productTypeId && x.Active && !x.Deleted && x.CompanyBranchId == loggedInUser.CompanyBranchId).FirstOrDefault();
					if (productTypeToEdit != null)
						return productTypeToEdit;
				}
			}
			return null;
		}
		public Product EditProductDetails(ProductViewModel productDetails)
		{
			var loggedInuser = Utitily.GetCurrentUser();
			if (loggedInuser.Id == null)
				loggedInuser = _userHelper.UpdateSessionAsync(loggedInuser.UserName)?.Result;

			if (loggedInuser != null)
			{
				if (productDetails != null)
				{
					var prosDetails = _context.Products.Where(x => x.Id == productDetails.Id && x.Active && !x.Deleted).FirstOrDefault();
					if (prosDetails != null)
					{
						prosDetails.Name = productDetails.Name;
						prosDetails.ProductTypeId = productDetails.ProductTypeId;
						_context.Update(prosDetails);
						_context.SaveChanges();
						return prosDetails;
					}
				}
			}
			return null;
		}
		public ProductType EditProductTypeDetails(ProductTypeViewModel productTypeDetails)
		{
			if (productTypeDetails != null)
			{
				var prosTypeDetails = _context.ProductTypes.Where(x => x.Id == productTypeDetails.Id && x.Active && !x.Deleted).FirstOrDefault();
				if (prosTypeDetails != null)
				{
					prosTypeDetails.Name = productTypeDetails.Name;
					prosTypeDetails.DateCreated = DateTime.Now;

					_context.Update(prosTypeDetails);
					_context.SaveChanges();
					return prosTypeDetails;
				}
			}
			return null;
		}
		public Product DeleteProductDetails(ProductViewModel productDetails)
		{
			if (productDetails != null)
			{
				var product = _context.Products.Where(x => x.Id == productDetails.Id && x.Active && !x.Deleted).FirstOrDefault();
				if (product != null)
				{
					product.Active = false;
					product.Deleted = true;

					_context.Update(product);
					_context.SaveChanges();
					return product;
				}
			}
			return null;
		}
		public ProductType DeleteProductTypeDetails(ProductTypeViewModel productTypeDetails)
		{
			if (productTypeDetails != null)
			{
				var productType = _context.ProductTypes.Where(x => x.Id == productTypeDetails.Id && x.Active && !x.Deleted).FirstOrDefault();
				if (productType != null)
				{

					productType.Deleted = true;
					productType.Active = false;

					_context.Update(productType);
					_context.SaveChanges();
					return productType;
				}
			}
			return null;
		}
		public bool PhoneNumberCheck(CustomerViewModel customerDetails)
		{
			if (customerDetails.PhoneNumber != null)
			{
				var loggedInuser = Utitily.GetCurrentUser();
				if (loggedInuser.Id == null)
					loggedInuser = _userHelper.UpdateSessionAsync(loggedInuser.UserName)?.Result;

				if (loggedInuser != null)
				{
					var checks = _context.Customers.Where(f => f.PhoneNumber == customerDetails.PhoneNumber && f.CompanyBranchId == loggedInuser.CompanyBranchId).FirstOrDefault();
					if (checks != null)
						return true;
				}

			}
			return false;
		}
		public bool EmailCheck(CustomerViewModel customerDetails)
		{
			if (customerDetails.Email != null)
			{
				var loggedInuser = Utitily.GetCurrentUser();
				if (loggedInuser.Id == null)
					loggedInuser = _userHelper.UpdateSessionAsync(loggedInuser.UserName)?.Result;

				if (loggedInuser != null)
				{
					var emailChecks = _context.Customers.Where(f => f.Email == customerDetails.Email && f.CompanyBranchId == loggedInuser.CompanyBranchId).FirstOrDefault();
					if (emailChecks != null)
						return true;
				}
			}
			return false;
		}
		public bool CustomerPostServer(CustomerViewModel customerDetails)
		{
			if (customerDetails != null)
			{
				var loggedInuser = Utitily.GetCurrentUser();
				if (loggedInuser.Id == null)
					loggedInuser = _userHelper.UpdateSessionAsync(loggedInuser.UserName)?.Result;

				if (loggedInuser != null)
				{
					var customerModel = new Customer();
					{
						customerModel.CompanyBranchId = loggedInuser.CompanyBranchId;
						customerModel.FirstName = customerDetails.FirstName;
						customerModel.Address = customerDetails.Address;
						customerModel.PhoneNumber = customerDetails.PhoneNumber;
						customerModel.LastName = customerDetails.LastName;
						customerModel.Email = customerDetails.Email;
						customerModel.IsDeleted = false;
						customerModel.Active = true;
						customerModel.Deleted = false;
						customerModel.DateCreated = DateTime.Now;
						_context.Customers.Add(customerModel);
						_context.SaveChanges();
						return true;
					}

				}
				return false;

			}
			return false;
		}
		public bool GetEditCustomer(CustomerViewModel customerDetails)
		{
			if (customerDetails != null)
			{

				var customerToEdit = _context.Customers.Where(c => c.Id == customerDetails.Id).FirstOrDefault();

				customerToEdit.FirstName = customerDetails.FirstName;
				customerToEdit.Address = customerDetails.Address;
				customerToEdit.PhoneNumber = customerDetails.PhoneNumber;
				customerToEdit.LastName = customerDetails.LastName;
				customerToEdit.Email = customerDetails.Email;

				_context.Customers.Update(customerToEdit);
				_context.SaveChanges();

				return true;
			}
			return false;
		}
		public bool DeleteCustomer(CustomerViewModel customerDetails)
		{
			if (customerDetails != null)
			{
				var customer = _context.Customers.Where(c => c.Id == customerDetails.Id).FirstOrDefault();

				customer.IsDeleted = true;
				customer.Active = false;

				_context.Customers.Update(customer);
				_context.SaveChanges();

				return true;
			}
			return false;
		}
		public List<Customer> GetCustomerList()
		{
			var customerViewModel = new List<Customer>();
			var loggedInuser = Utitily.GetCurrentUser();
			if (loggedInuser.Id == null)
			{
				loggedInuser = _userHelper.UpdateSessionAsync(loggedInuser.UserName).Result;

			}
			if (loggedInuser != null)
			{
				var customerList = _context.Customers.Where(c => !c.IsDeleted && c.CompanyBranchId == loggedInuser.CompanyBranchId).ToList();
				return customerList;
			}
			return customerViewModel;
		}
		public bool CreateBookingGroup(BookingGroupViewModel bookingGroupDetails)
		{
			if (bookingGroupDetails != null)
			{
				var loggedInuser = Utitily.GetCurrentUser();
				if (loggedInuser.Id == null)
				{
					loggedInuser = _userHelper.UpdateSessionAsync(loggedInuser.UserName).Result;

				}
				if (loggedInuser != null)
				{
					var checkGroupName = _context.BookingGroups.Where(c => c.Name == bookingGroupDetails.Name
							&& c.CompanyBranchId == loggedInuser.CompanyBranchId).FirstOrDefault();
					if (checkGroupName == null)
					{
						bookingGroupDetails.ExpectedPrice = bookingGroupDetails.ExpectedCostPrice;
						var bookingGroupModel = new BookingGroup()
						{
							Name = bookingGroupDetails.Name,
							ExpectedDateOfArrival = bookingGroupDetails.ExpectedDateOfArrivalD,
							ExpectedPrice = bookingGroupDetails.ExpectedPrice,
							ExpectedCostPrice = bookingGroupDetails.ExpectedCostPrice,
							ProductId = bookingGroupDetails.ProductId,
							MotalityRecorded = bookingGroupDetails.MotalityRecorded,
							QuantityArrived = bookingGroupDetails.QuantityArrived,
							QuantityLeft = bookingGroupDetails.QuantityLeft,
							QuantitySold = bookingGroupDetails.QuantitySold,
							CompanyBranchId = loggedInuser.CompanyBranchId,
							Active = true,
							Deleted = false,
							DateCreated = DateTime.Now,
						};
						_context.BookingGroups.Add(bookingGroupModel);
						_context.SaveChanges();
						return true;
					}

				}

			}

			return false;
		}
		public List<BookingGroupViewModel> GetBookingGroupList()
		{
			var listOfBookings = new List<BookingGroupViewModel>();
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;
			}
			if (loggedInUser != null)
			{
				var bookingGroupList = _context.BookingGroups.Where(c => !c.Deleted && c.ExpectedDateOfArrival.Value.Date >= DateTime.Now.Date && c.CompanyBranchId == loggedInUser.CompanyBranchId).Include(x => x.CompanyBranch)
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

		//Edit BoookingGroup//
		public bool EditBookingGroup(BookingGroupViewModel bookingGroupDetails)
		{
			if (bookingGroupDetails != null)
			{
				var bookingGroupToEdit = _context.BookingGroups.Where(c => c.Id == bookingGroupDetails.Id).FirstOrDefault();
				if (bookingGroupToEdit != null)
				{
					bookingGroupToEdit.Name = bookingGroupDetails.Name;
					bookingGroupToEdit.ExpectedCostPrice = bookingGroupDetails.ExpectedCostPrice;
					bookingGroupToEdit.ExpectedPrice = bookingGroupDetails.ExpectedCostPrice;
					bookingGroupToEdit.ProductId = bookingGroupDetails.ProductId;
				}
				_context.BookingGroups.Update(bookingGroupToEdit);
				_context.SaveChanges();
				return true;
			}
			return false;
		}

		public bool DeleteBookingGroup(BookingGroupViewModel bookingGroupDetails)
		{
			if (bookingGroupDetails != null)
			{
				var bookingGroup = _context.BookingGroups.Where(c => c.Id == bookingGroupDetails.Id).FirstOrDefault();
				if (bookingGroup != null)
				{
					bookingGroup.Deleted = true;
					bookingGroup.Active = false;
				}
				_context.BookingGroups.Update(bookingGroup);
				_context.SaveChanges();
				return true;
			}
			return false;
		}
		public bool UpdateArrivalReceipt(BookingGroupViewModel bookingGroupDetails)
		{
			if (bookingGroupDetails != null)
			{
				var arrivalDetails = _context.BookingGroups.Where(c => c.Id == bookingGroupDetails.Id).FirstOrDefault();
				if (arrivalDetails != null)
				{
					arrivalDetails.SupplyDate = DateTime.Now;
					arrivalDetails.MotalityRecorded = bookingGroupDetails.MotalityRecorded;
					arrivalDetails.ExpectedPrice = bookingGroupDetails.ExpectedPrice;
					arrivalDetails.QuantitySold = bookingGroupDetails.QuantitySold;
					arrivalDetails.QuantityArrived = bookingGroupDetails.QuantityArrived;
					arrivalDetails.SupplyPrice = bookingGroupDetails.SupplyPrice;
					arrivalDetails.QuantityLeft = bookingGroupDetails.QuantityLeft;

				}
				_context.BookingGroups.Update(arrivalDetails);
				_context.SaveChanges();
				return true;
			}
			return false;
		}
		public List<StaffViewModel> GetAvailableRoles()
		{
			var staffViewModel = new List<StaffViewModel>();
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

			}
			if (loggedInUser != null)
			{
				var result = new List<StaffViewModel>();
				var roles = _context.Roles.Where(c => c.Name != loggedInUser.Roles.First()).ToList();
				foreach (var item in roles.Where(s => s.Name != "SuperAdmin"))
				{
					if (item.Name == Utitily.Constants.CompanyStaffRole)
						item.Name = "Company Staff";
					else if (item.Name == Utitily.Constants.CompanyManagerRole)
						item.Name = "Company Manager";

					var role = new StaffViewModel()
					{
						Name = item.Name,
						Id = item.Id,
						BranchId = loggedInUser.CompanyBranchId
					};
					result.Add(role);
				}
				return result;
			}
			return staffViewModel;
		}

		public StaffViewModel GetAllScreensFromEnum(string roleId)
		{
			var result = new StaffViewModel();
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

			}
			if (loggedInUser != null)
			{
				var availableViews = ((ScreenEnums[])Enum.GetValues(typeof(ScreenEnums)))
				.Select(c => new DropdownEnumModel() { Id = (int)c, Name = c.ToString() }).ToList();

				foreach (var view in availableViews)
				{
					var roleAccessGranted = _context.ScreenRoles.Where(x => x.CompanyBranchId == loggedInUser.CompanyBranchId &&
							x.RoleId == roleId && x.ScreenId == view.Id).FirstOrDefault();
					if (roleAccessGranted != null)
					{
						view.Ischecked = roleAccessGranted.ScreenChecked;
					}
				}
				var roleName = _context.Roles.Find(roleId);
				if (roleName?.Name == Utitily.Constants.CompanyStaffRole)
					result.RoleName = "Staff";
				else if (roleName?.Name == Utitily.Constants.CompanyManagerRole)
					result.RoleName = "Manager";
				if (availableViews.Count != 0)
				{
					result.BranchId = loggedInUser.CompanyBranchId;
					result.RoleAccess = availableViews;
					result.Id = roleId;
					return result;
				}
			}
			return result;
		}

		public string UpdateRoleAccess(StaffViewModel details, List<string> checkedRole, List<string> uncheckedRole)
		{
			if (checkedRole.Count > 0 || uncheckedRole.Count > 0)
			{
				foreach (var screen in checkedRole)
				{
					var roleAccessGranted = _context.ScreenRoles.Where(x => x.CompanyBranchId == details.BranchId &&
						x.RoleId == details.Id && x.Name == screen).FirstOrDefault();
					if (roleAccessGranted == null)
					{
						var screenId = (int)(ScreenEnums)Enum.Parse(typeof(ScreenEnums), screen);
						var accessScreen = new ScreenRole()
						{
							Active = true,
							CompanyBranchId = details.BranchId,
							ScreenChecked = true,
							Name = screen,
							DateAssigned = DateTime.Now,
							Deleted = false,
							ScreenId = screenId,
							RoleId = details.Id,
						};
						_context.ScreenRoles.Add(accessScreen);
					}
					else
					{
						roleAccessGranted.Active = true;
						roleAccessGranted.ScreenChecked = true;
						roleAccessGranted.Deleted = false;
						roleAccessGranted.DateAssigned = DateTime.Now;
						_context.ScreenRoles.Update(roleAccessGranted);

					}
				}

				foreach (var screen in uncheckedRole)
				{
					var roleAccessGranted = _context.ScreenRoles.Where(x => x.CompanyBranchId == details.BranchId &&
						x.RoleId == details.Id && x.Name == screen).FirstOrDefault();
					if (roleAccessGranted != null)
					{
						roleAccessGranted.Active = false;
						roleAccessGranted.ScreenChecked = false;
						roleAccessGranted.Deleted = true;
						_context.ScreenRoles.Update(roleAccessGranted);
					}
				}
				_context.SaveChanges();
				return details.Id;
			}
			return null;
		}

		public List<RoleViewModel> GetStaffRoles()
		{
			var userRoles = new List<RoleViewModel>();
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

			}
			if (loggedInUser != null)
			{
				var roles = _context.UserRoles.ToList();
				foreach (var userRole in roles)
				{
					var user = _userManager.Users.Where(x => x.CompanyBranchId == loggedInUser.CompanyBranchId && userRole.UserId == x.Id && x.Id != loggedInUser.Id).FirstOrDefault();
					if (user != null)
					{
						var userRoleName = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
						var role = _context.Roles.Where(r => r.Name == userRoleName).FirstOrDefault();
						var roleViewModel = new RoleViewModel();
						roleViewModel.UserName = user.FullName;
						roleViewModel.StaffId = user.Id;
						roleViewModel.RoleName = role.Name;
						roleViewModel.RoleId = role.Id;
						userRoles.Add(roleViewModel);

					}
					continue;
				}
			}
			return userRoles;
		}

		public string ImpersonateCompanyBranch(Guid companyBranchId)
		{
			if (companyBranchId != Guid.Empty)
			{
				var companyBranch = _context.CompanyBranches.Where(x => x.Id == companyBranchId && x.Active).FirstOrDefault();
				if (companyBranch != null)
				{
					var loggedInUser = Utitily.GetCurrentUser();
					if (loggedInUser.Id == null)
					{
						loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

					}
					if (loggedInUser != null)
					{
						loggedInUser.CompanyBranchId = companyBranchId;
						loggedInUser.BranchName = companyBranch.Name;
						var currentUser = JsonConvert.SerializeObject(loggedInUser);
						return currentUser;
					}
				}
			}
			return null;
		}



		public List<MeasurementUnitViewModel> GetUnitList()
		{
			var measurementUnitViewModel = new List<MeasurementUnitViewModel>();
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName)?.Result;
			}
			if (loggedInUser != null)
			{
				var unitList = _context.Measurementunits.Where(c => !c.Deleted && c.CompanyBranchId == loggedInUser.CompanyBranchId).ToList()
				.Select(x => new MeasurementUnitViewModel
				{
					Name = x.Name,
					Id = x.Id,
					DateCreated = x.DateCreated,
					CompanyBranchId = x.CompanyBranchId,
				}).ToList();
				if (unitList != null && unitList.Count > 0)
				{
					return unitList;
				}
			}
			return measurementUnitViewModel;
		}


		public bool CreateMeasurementUnit(MeasurementUnitViewModel UnitDetails)
		{
			if (UnitDetails != null)
			{
				var loggedInuser = Utitily.GetCurrentUser();
				if (loggedInuser.Id == null)
				{
					loggedInuser = _userHelper.UpdateSessionAsync(loggedInuser.UserName).Result;

				}
				if (loggedInuser != null)
				{
					var checkUnitName = _context.Measurementunits.Where(c => c.Name == UnitDetails.Name
							&& c.CompanyBranchId == loggedInuser.CompanyBranchId).FirstOrDefault();
					if (checkUnitName == null)
					{
						var measurementUnitModel = new Measurementunit()
						{
							Name = UnitDetails.Name,
							CompanyBranchId = loggedInuser.CompanyBranchId,
							Active = true,
							Deleted = false,
							DateCreated = DateTime.Now,
						};
						_context.Measurementunits.Add(measurementUnitModel);
						_context.SaveChanges();
						return true;
					}
				}
			}
			return false;
		}

		public bool EditMeasurementUnit(MeasurementUnitViewModel UnitDetails)
		{
			if (UnitDetails != null)
			{
				var MeasurementUnitToEdit = _context.Measurementunits.Where(c => c.Id == UnitDetails.Id).FirstOrDefault();
				if (MeasurementUnitToEdit != null)
				{
					MeasurementUnitToEdit.Name = UnitDetails.Name;
					MeasurementUnitToEdit.DateCreated = UnitDetails.DateCreated;

				}
				_context.Measurementunits.Update(MeasurementUnitToEdit);
				_context.SaveChanges();
				return true;
			}
			return false;
		}


		public bool DeleteMeasurementUnit(MeasurementUnitViewModel UnitDetails)
		{
			if (UnitDetails != null)
			{
				var measurementUnit = _context.Measurementunits.Where(c => c.Id == UnitDetails.Id).FirstOrDefault();
				if (measurementUnit != null)
				{
					measurementUnit.Deleted = true;
					measurementUnit.Active = false;
				}
				_context.Measurementunits.Update(measurementUnit);
				_context.SaveChanges();
				return true;
			}
			return false;
		}

		//Get Product type
		public List<ShopProductViewModel> GetProductCategoryList()
		{
			var ShopProductViewModel = new List<ShopProductViewModel>();
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName)?.Result;

			if (loggedInUser != null)
			{
				var productCategory = _context.ShopCategories
			   .Where(x => x.Id != 0 && !x.Deleted && x.CompanyBranchId == loggedInUser.CompanyBranchId)
			   .Select(s => new ShopProductViewModel()
			   {
				   Id = s.Id,
				   Name = s.Name,
				   Active = s.Active,
				   DateCreated = s.DateCreated
			   }).ToList();
				return productCategory;
			}
			return ShopProductViewModel;
		}

		public bool CreateShopProductDetails(ShopProductViewModel shopProductViewModel)
		{
			var loggedInuser = Utitily.GetCurrentUser();
			if (loggedInuser.Id == null)
			{
				loggedInuser = _userHelper.UpdateSessionAsync(loggedInuser.UserName).Result;

			}
			if (loggedInuser != null)
			{
				var productCategoryValidation = _context.ShopCategories.Where(c => c.Name == shopProductViewModel.Name && c.CompanyBranchId == loggedInuser.CompanyBranchId && !c.Deleted).FirstOrDefault();
				if (productCategoryValidation == null)
				{
					var ShopProductModel = new ShopCategory();
					ShopProductModel.CompanyBranchId = loggedInuser.CompanyBranchId;
					ShopProductModel.Name = shopProductViewModel.Name;
					ShopProductModel.DateCreated = DateTime.Now;
					ShopProductModel.Active = true;
					ShopProductModel.Deleted = false;

					_context.ShopCategories.Add(ShopProductModel);
					_context.SaveChanges();
					return true;
				}
			}
			return false;
		}
		public ShopCategory EditProductCatDetails(int productCatId)
		{
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

			}
			if (loggedInUser != null)
			{
				if (productCatId != 0)
				{
					var productToEdit = _context.ShopCategories.Where(x => x.Id == productCatId && x.Active && !x.Deleted && x.CompanyBranchId == loggedInUser.CompanyBranchId).FirstOrDefault();
					if (productToEdit != null)
						return productToEdit;
				}
			}
			return null;
		}
		public bool EditProductCAtDetails(ShopProductViewModel productCatDetails)
		{
			if (productCatDetails != null)
			{
				var prosCatDetails = _context.ShopCategories.Where(x => x.Id == productCatDetails.Id && x.Active && !x.Deleted).FirstOrDefault();
				if (prosCatDetails != null)
				{
					prosCatDetails.Name = productCatDetails.Name;

					_context.Update(prosCatDetails);
					_context.SaveChanges();
					return true;
				}
			}
			return false;
		}
		public bool DeleteProductCatDetails(ShopProductViewModel productCatDetails)
		{
			if (productCatDetails != null)
			{
				var productCat = _context.ShopCategories.Where(x => x.Id == productCatDetails.Id && x.Active && !x.Deleted).FirstOrDefault();
				if (productCat != null)
				{

					productCat.Deleted = true;
					productCat.Active = false;

					_context.Update(productCat);
					_context.SaveChanges();
					return true;
				}
			}
			return false;
		}

		public List<Supplier> GetSuppliers()
		{
			var common = new Supplier()
			{
				Id = 0,
				Name = "-- Select --"
			};
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;
			}

			var supplierslists = _context.Suppliers.Where(x => x.CompanyBranchId == loggedInUser.CompanyBranchId && x.Active).ToList();
			//var groups = _mapper.Map<List<ProductInventoriesViewModel>>(supplierslists);
			supplierslists.Insert(0, common);
			return supplierslists;
		}

		public List<Measurementunit> GetUnits()
		{
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

			}
			var common = new Measurementunit()
			{
				Id = 0,
				Name = "-- Select --"
			};
			var units = _context.Measurementunits.Where(x => !x.Deleted && x.CompanyBranchId == loggedInUser.CompanyBranchId).ToList();
			// var unitGroups = _mapper.Map<List<ProductInventoryViewModel>>(units);
			units.Insert(0, common);
			return units;
		}

		public List<ShopCategory> GetProductCategories()
		{
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;
			}
			var common = new ShopCategory()
			{
				Id = 0,
				Name = "-- Select --"
			};
			var productCategories = _context.ShopCategories.Where(x => !x.Deleted && x.CompanyBranchId == loggedInUser.CompanyBranchId).ToList();
			//var productGroups = _mapper.Map<List<ProductInventoryViewModel>>(productCategories);
			productCategories.Insert(0, common);
			return productCategories;
		}
		public List<ProductInventory> GetShopProducts()
		{
			var inventories = new List<ProductInventory>();
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;
			}
			if (loggedInUser != null)
			{
				var common = new ProductInventory()
				{
					Id = 0,
					Name = "-- Select --"
				};
				var products = _context.ProductInventories.Where(x => !x.Deleted && x.CompanyBranchId == loggedInUser.CompanyBranchId).ToList();
				products.Insert(0, common);
				return products;
			}
			return inventories;
		}


		public List<ShopProductListViewModel> GetShopProductList()
		{
			var shopProductListViewModel = new List<ShopProductListViewModel>();
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;
			}
			if (loggedInUser != null)
			{
				var productCategory = _context.ProductInventories
			   .Where(x => x.Id != 0 && !x.Deleted && x.CompanyBranchId == loggedInUser.CompanyBranchId).Include(x => x.Supplier).Include(p => p.ShopCategory).Include(r => r.Measurementunit)
			   .Select(s => new ShopProductListViewModel()
			   {
				   Id = s.Id,
				   Name = s.Name,
				   ProductName = s.Name,
				   SupplierName = s.Supplier.Name,
				   UnitName = s.Measurementunit.Name,
				   ProductCategoryName = s.ShopCategory.Name,
				   PricePerUnit = s.AmountPerProduct,
				   QuantityBought = s.Quantity,
				   TotalAmountofQuantityBought = s.Amount,
				   UserId = loggedInUser.Id,
			   }).ToList();
				return productCategory;
			}
			return shopProductListViewModel;
		}

		public List<BookingGroupViewModel> GetAllBookingGroup()
		{
			var listOfBookings = new List<BookingGroupViewModel>();
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;
			}
			if (loggedInUser != null)
			{
				var bookingGroupList = _context.BookingGroups.Where(c => !c.Deleted && c.CompanyBranchId == loggedInUser.CompanyBranchId).Include(x => x.CompanyBranch)
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


		public List<ProductInventoryViewModel> GetProductLogView(int productInventoryId)
		{
			var productLogViewModel = new List<ProductInventoryViewModel>();
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;
			}
			if (loggedInUser != null)
			{
				if (productInventoryId > 0)
				{

					productLogViewModel = _context.ProductLogs
			        .Where(x => x.Id != 0 && !x.Deleted && x.CompanyBranchId == loggedInUser.CompanyBranchId && x.ProductInventoryId == productInventoryId).Include(p => p.ProductInventory)
			        .Select(s => new ProductInventoryViewModel()
			        {
						Id = s.Id,
						Name = s.Name,
						DateCreated = s.DateCreated,
						Quantity = s.Qauntity,
						NewQuantity = s.NewQuantity,
						OldQauntity = s.OldQauntity,
						AmountPerProduct = s.ProductInventory.Amount,
						AccessName = s.User.FullName,
						UserId = s.UserId,
						ProductInventoryId = s.ProductInventoryId,
						SellingPrice = s.SellingPrice,
						ProductActivity = s.ProductActivity,
					
					}).ToList();

				}
				else
				{
					productLogViewModel = _context.ProductLogs
					.Where(x => x.Id != 0 && !x.Deleted && x.CompanyBranchId == loggedInUser.CompanyBranchId).Include(p => p.ProductInventory)
					.Select(s => new ProductInventoryViewModel()
					{
						Id = s.Id,
						Name = s.Name,
						DateCreated = s.DateCreated,
						Quantity = s.Qauntity,
						NewQuantity = s.NewQuantity,
						OldQauntity = s.OldQauntity,
						AmountPerProduct = s.ProductInventory.Amount,
						AccessName = s.User.FullName,
						UserId = s.UserId,
						ProductInventoryId = s.ProductInventoryId,
						SellingPrice = s.SellingPrice,
						ProductActivity = s.ProductActivity
					}).ToList();

				}
			}
			return productLogViewModel;
		}

        public ShopProductListViewModel GetProductInventoryById(int id)
        {
            var productInventory = new ShopProductListViewModel();
            if (id > 0)
            {
                var productInventoryToBeEdited = _context.ProductInventories.Where(x => x.Id == id && x.Active && !x.Deleted).Include(x => x.Supplier).Include(x => x.Measurementunit).Include(x => x.ShopCategory).FirstOrDefault();
                if (productInventoryToBeEdited != null)
                {
					var shopProductInventory = new ShopProductListViewModel()
					{
						Id = productInventoryToBeEdited.Id,
						SupplierName = productInventoryToBeEdited.Supplier?.Name,
						SupplierId = productInventoryToBeEdited.SupplierId,
                        ShopCategoryId = productInventoryToBeEdited.ShopCategoryId,
						ProductName = productInventoryToBeEdited.Name,
						ProductCategoryName = productInventoryToBeEdited.ShopCategory?.Name,
						MeasurementunitId = productInventoryToBeEdited.MeasurementunitId,
						UnitName = productInventoryToBeEdited.Measurementunit?.Name,
						QuantityBought = productInventoryToBeEdited.Quantity,
						TotalAmountofQuantityBought = productInventoryToBeEdited.Amount,
						PricePerUnit = productInventoryToBeEdited.AmountPerProduct,
					};
                    return shopProductInventory;
                }
            }
            return productInventory;
        }

        public string EditProductInventory(ShopProductListViewModel shopProductListViewModel)
        {
            if (shopProductListViewModel != null)
            {
                var productInventories = _context.ProductInventories.Where(x => x.Id == shopProductListViewModel.Id && x.Active && !x.Deleted).Include(x => x.Supplier).Include(x => x.Measurementunit).Include(x => x.ShopCategory).FirstOrDefault();
                if (productInventories != null)
                {

                    productInventories.SupplierId = shopProductListViewModel.SupplierId;
                    productInventories.ShopCategoryId = shopProductListViewModel.ShopCategoryId;
                    productInventories.MeasurementunitId = shopProductListViewModel.MeasurementunitId;
                    productInventories.Name = shopProductListViewModel.ProductName;
                    productInventories.Amount = shopProductListViewModel.TotalAmountofQuantityBought;
                    productInventories.AmountPerProduct = shopProductListViewModel.PricePerUnit;
                    productInventories.Quantity = shopProductListViewModel.QuantityBought;
                    _context.ProductInventories.Update(productInventories);
                    _context.SaveChanges();
                    return "ProductInventory Successfully Updated";
                }
            }
            return "ProductInventory failed To Update";
        }

        public string DeleteProductInventory(int id)
        {
            if (id != 0)
            {
                var productInventories = _context.ProductInventories.Where(x => x.Id == id && x.Active && !x.Deleted).FirstOrDefault();
                if (productInventories != null)
                {
                    productInventories.Active = false;
                    productInventories.Deleted = true;
                    _context.ProductInventories.Update(productInventories);
                    _context.SaveChanges();
                    return "ProductInventory Successfully Deleted";
                }
            }
            return "ProductInventory failed To Delete";
        }


		public bool CreateRoutine(RoutineViewModel routineDetails)
		{
			if (routineDetails != null)
			{
				var loggedInuser = Utitily.GetCurrentUser();
				if (loggedInuser.Id == null)
				{
					loggedInuser = _userHelper.UpdateSessionAsync(loggedInuser.UserName).Result;

				}
				if (loggedInuser != null)
				{
					var checkRoutineName = _context.Routines.Where(c => c.Id == Guid.Empty && c.CompanyBranchId == loggedInuser.CompanyBranchId).FirstOrDefault();
					if (checkRoutineName == null)
					{
						var routineModel = new Routine()
						{
							
							Active = true,
							Id = routineDetails.Id,
							PatientId = routineDetails.PatientId,
							CompanyBranchId = loggedInuser.CompanyBranchId,
							Purpose = routineDetails.Purpose,
							EmailAllowed = routineDetails.EmailAllowed,
							SMSAllowed = routineDetails.SMSAllowed,
							CurrentDate =DateTime.Now,
							NextDate = routineDetails.NextDate,
							
						};
						_context.Routines.Add(routineModel);
						_context.SaveChanges();
						return true;
					}

				}

			}
			return false;
		}


		public List<RoutineViewModel> GetRoutineList()
		{
			var listOfRoutine = new List<RoutineViewModel>();
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;
			}
			if (loggedInUser != null)
			{
				var routineList = _context.Routines.Where(c => c.Active && c.CompanyBranchId == loggedInUser.CompanyBranchId)
				.Select(x => new RoutineViewModel
				{
					Id = x.Id,
					CompanyBranchId = x.CompanyBranchId,
					CurrentDate = x.CurrentDate,
					NextDate = x.NextDate,
					Active = x.Active,
					EmailAllowed = x.EmailAllowed,
					PatientId = x.PatientId,
					Purpose = x.Purpose,
					SMSAllowed = x.SMSAllowed,

				}).ToList();
				if (routineList != null && routineList.Count > 0)
				{
					return routineList;
				}
			}
			return listOfRoutine;
		}

		public bool EditRoutine(RoutineViewModel routineDetails)
		{
			if (routineDetails != null)
			{
				var routineToEdit = _context.Routines.Where(c => c.Id == routineDetails.Id).FirstOrDefault();
				if (routineToEdit != null)
				{
					routineToEdit.Purpose = routineDetails.Purpose;
					routineToEdit.EmailAllowed = routineDetails.EmailAllowed;
					routineToEdit.CurrentDate = routineDetails.CurrentDate;
					routineToEdit.NextDate = routineDetails.NextDate;
					routineToEdit.PatientId = routineDetails.PatientId;
					routineToEdit.Id = routineDetails.Id;
					routineToEdit.SMSAllowed = routineDetails.SMSAllowed;
					_context.Routines.Update(routineToEdit);
					_context.SaveChanges();
					return true;
				}
			}
			return false;
		}

		public bool DeleteRoutine(RoutineViewModel routineDetails)
		{
			if (routineDetails != null)
			{
				var routineToDelete = _context.Routines.Where(c => c.Id == routineDetails.Id && c.Active).FirstOrDefault();
				if (routineToDelete != null)
				{
					routineToDelete.Active = false;
				}
				_context.Routines.Update(routineToDelete);
				_context.SaveChanges();
				return true;
			}
			return false;
		}

		public List<VisitViewModel> AllPatientVisits(ApplicationUser loggednUser)
		{
			try
			{
                var visitViewModels = new List<VisitViewModel>();
                var visits = _context.Visits.Where(x => x.Id != 0 && x.PatientId != 0 && x.Active && !x.Deleted && x.CompanyBranchId == loggednUser.CompanyBranchId).Include(x => x.CreatedBy).Include(x => x.Patient).Include(x=>x.Patient.Breed).Include(x => x.CompanyBranch).ToList();
                if (visits != null && visits.Count > 0)
                {
                    var patientVisits = visits.Select(s => new VisitViewModel()
                    {
                        Id = s.Id,
                        DefinitiveDiagnosis = s.DefinitiveDiagnosis,
                        PatientId = s.PatientId,
                        PatientName = s.Patient?.Name,
                        PatientPicture = s.Patient?.Picture,
                        CreatedName = s.CreatedBy?.FullName,
						CaseNumber = s.Patient?.CaseNumber,
						Breed = s.Patient?.Breed?.Name,
                        CreatedById = s.CreatedById,
                    }).ToList();
                    return patientVisits;
                }
                return visitViewModels;
            }
            catch (Exception ex)
            {
                string toEmail = _generalConfiguration.DeveloperEmail;
                string subject = "Exception On AllPatientVisits";
                string message = ex.Message + " , <br /> This exception message occurred while trying to get All Patient Visits";
                _emailService.SendEmail(toEmail, subject, message);
                throw;
            }
		}
        public VisitViewModel PatientVisitById(int patientId , ApplicationUser loggedInUser)
        {
			try
			{
                var visitViewModel = new VisitViewModel();
                var visits = _context.Visits.Where(x => x.Id != 0 && x.PatientId == patientId && x.Active && !x.Deleted && x.CompanyBranchId == loggedInUser.CompanyBranchId).Include(x => x.CreatedBy).Include(x => x.Patient).Include(x => x.CompanyBranch).FirstOrDefault();
                if (visits != null)
                {
                    var patientVisit = new VisitViewModel()
                    {
                        Id = visits.Id,
                        CompanyBranchId = visits.CompanyBranchId,
                        TreatmentHistory = visits.TreatmentHistory,
                        DefinitiveDiagnosis = visits.DefinitiveDiagnosis,
                        EnvironmentalHistory = visits.EnvironmentalHistory,
                        FeedingHistory = visits.FeedingHistory,
                        GeneralExamination = visits.GeneralExamination,
                        LaboratoryExamination = visits.LaboratoryExamination,
                        LaboratoryResults = visits.LaboratoryResults,
                        LaboratorySamples = visits.LaboratorySamples,
                        DifferentialDiagnosis = visits.DifferentialDiagnosis,
                        PhysicalExamination = visits.PhysicalExamination,
                        PhysiologicalExamination = visits.PhysiologicalExamination,
                        VaccinationHistory = visits.VaccinationHistory,
                        PatientId = visits.PatientId,
                        PatientName = visits.Patient?.Name,
                        PatientPicture = visits.Patient?.Picture,
                        CreatedName = visits.CreatedBy?.FullName,
                        CreatedById = visits.CreatedById,
						Breed = visits.Patient?.Breed?.Name,

					};
                    return patientVisit;
                }
                return visitViewModel;
            }
            catch (Exception ex)
            {
                string toEmail = _generalConfiguration.DeveloperEmail;
                string subject = "Exception On Patient Visit By Id";
                string message = ex.Message + " , <br /> This exception message occurred while trying to get Patient Visit By Id";
                _emailService.SendEmail(toEmail, subject, message);
                throw;
            }

        }
		public async Task<Visit> AddPatientVisit(VisitViewModel visitViewModel, ApplicationUser loggedInUser)
		{
			try
			{
                if (loggedInUser != null)
                {
                    var visitModel = new Visit()
                    {
						
                        CompanyBranchId = loggedInUser.CompanyBranchId,
                        TreatmentHistory = visitViewModel.TreatmentHistory,
                        DefinitiveDiagnosis = visitViewModel.DefinitiveDiagnosis,
                        EnvironmentalHistory = visitViewModel.EnvironmentalHistory,
                        FeedingHistory = visitViewModel.FeedingHistory,
                        GeneralExamination = visitViewModel.GeneralExamination,
                        LaboratoryExamination = visitViewModel.LaboratoryExamination,
                        LaboratoryResults = visitViewModel.LaboratoryResults,
                        LaboratorySamples = visitViewModel.LaboratorySamples,
                        DifferentialDiagnosis = visitViewModel.DifferentialDiagnosis,
                        PhysicalExamination = visitViewModel.PhysicalExamination,
                        PhysiologicalExamination = visitViewModel.PhysiologicalExamination,
                        VaccinationHistory = visitViewModel.VaccinationHistory,
                        PatientId = visitViewModel.PatientId,
                        CreatedById = loggedInUser.Id,
						PrimaryComplaint = visitViewModel.PrimaryComplaint,
                        DateCreated = DateTime.Now,
                        Active = true,
                        Deleted = false,
                    };
                    _context.Visits.Add(visitModel);
                    _context.SaveChanges();
					if (visitViewModel?.NextDate != null)
					{
					await AddPatientsRoutine(visitViewModel, loggedInUser.CompanyBranchId).ConfigureAwait(false);
					}
					return visitModel;
                }
                return null;
            }
            catch (Exception ex)
            {
                string toEmail = _generalConfiguration.DeveloperEmail;
                string subject = "Exception On Add Patient Visit";
                string message = ex.Message + " , <br /> This exception message occurred while trying to Add Patient Visit";
                _emailService.SendEmail(toEmail, subject, message);
                throw;
            }
		}
		public async Task<bool> AddVisitTreatment(List<VisitTreatmentViewModel> visitTreatments, Guid? companyBranchId, string createdById)
		{
			try
			{
				var visitTreatmentViewModels = new List<VisitTreatment>();
				if (visitTreatments != null && visitTreatments.Count > 0)
				{
					foreach (var visitTreatment in visitTreatments)
					{
						var visitTreatmentModel = new VisitTreatment()
						{
							VisitId = visitTreatment.VisitId,
							Discount =visitTreatment.Discount,
							TreatmentId =visitTreatment.TreatmentId,
							Cost = visitTreatment.Cost,
							CompanyBranchId = companyBranchId,
							CreatedById = createdById,
						};
						visitTreatmentViewModels.Add(visitTreatmentModel);
					}
					await _context.AddRangeAsync(visitTreatmentViewModels).ConfigureAwait(false);
					await _context.SaveChangesAsync().ConfigureAwait(false);
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				string toEmail = _generalConfiguration.DeveloperEmail;
				string subject = "Exception On Add Visit Treatment";
				string message = ex.Message + " , <br /> This exception message occurred while trying to Add Visit Treatment";
				_emailService.SendEmail(toEmail, subject, message);
				throw;
			}
		}
		public async Task<bool> AddPatientsRoutine(VisitViewModel visitViewModel, Guid? companyBranchId)
		{
			if (visitViewModel !=null && visitViewModel.Purpose != null)
			{
				var routine = new Routine()
				{
					PatientId = visitViewModel.PatientId,
					CurrentDate = DateTime.Now,
					NextDate = visitViewModel.NextDate,
					Purpose = visitViewModel.Purpose,
					Active = true,
					SMSAllowed = visitViewModel.SMSAllowed,
					EmailAllowed = visitViewModel.EmailAllowed,
					CompanyBranchId = companyBranchId,
				};
				await _context.AddAsync(routine).ConfigureAwait(false);
				await _context.SaveChangesAsync().ConfigureAwait(false);
				return true;
			}
			return false;
		}
	}
}
