using Core.Db;
using Core.Models;
using Core.ViewModels;
using Logic.IHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Xml.Linq;
using static Core.Enums.VetManEnums;

namespace Logic.Helpers
{
    public class DropdownHelper : IDropdownHelper
    {
        private readonly IEmailHelper _emailHelper;
        private readonly ICompanyHelper _companyHelper;
        private readonly ISubscriptionHelper subscriptionHelper;
        private readonly AppDbContext _context;
        private readonly IUserHelper _userHelper;
        private UserManager<ApplicationUser> _userManager;

        public DropdownHelper(AppDbContext context, UserManager<ApplicationUser> userManager, IUserHelper userHelper, 
            IEmailHelper emailHelper, ICompanyHelper companyHelper, ISubscriptionHelper subscriptionHelper)
        {
            _context = context;
            _emailHelper = emailHelper;
            _userHelper = userHelper;
            _userManager = userManager;
            _companyHelper = companyHelper;
            this.subscriptionHelper = subscriptionHelper;
        }

        public List<Product> GetProducts()
        {
            var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

			}
			var common = new Product()
            {
                Id = 0,
                Name = "-- Select --"
            };
            var products = _context.Products.Where(x => !x.Deleted && x.CompanyBranchId == loggedInUser.CompanyBranchId && x.Id != 0)
                .Select(x => new Product()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
            products.Insert(0, common);
            return products;
        }

        public List<ApplicationUser> GetAllStaff()
        {
            var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

			}
			var common = new ApplicationUser()
            {
                Id = "0000-000",
                UserName = "-- Select --"
            };
            var users = _userManager.Users.Where(c => c.Id != loggedInUser.Id && c.CompanyBranchId == loggedInUser.CompanyBranchId && !c.IsDeactivated )
                .Select(c => new ApplicationUser()
                {
                    Id = c.Id,
                    MiddleName = c.FirstName + " " + c.LastName,
                }).ToList();
            //ApplicationUser.(0, common);
            return users;
        }


		

		public List<CompanyBranch> GetAllCompanyBranch()
        {
            var listOfBranch = new List<CompanyBranch>();
            var common = new CompanyBranch()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                Name = "Switch Branch"
            };
            var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

			}
			var company = _context.CompanyBranches.Where(x => x.Id == loggedInUser.CompanyBranchId && x.CompanyId != Guid.Empty).Include(x => x.Company).FirstOrDefault();
            if (company != null)
            {
                var branches = _context.CompanyBranches.Where(x => !x.Deleted && x.CompanyId == company.CompanyId).ToList();
                branches.Insert(0, common);
                return branches;
            }
            listOfBranch.Insert(0, common);
            return listOfBranch;
        }

        public ProductInventory GetProductDetails(int id)
        {
            var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

			}
			var product = new ProductInventory();
            if (loggedInUser != null)
            {
                product = _context.ProductInventories.Where(x => !x.Deleted && x.Id == id &&
                   x.CompanyBranchId == loggedInUser.CompanyBranchId)?.Include(c => c.Supplier)?.Include(f => f.Measurementunit)?.FirstOrDefault();
            }
          
            return product;
        }


		public List<ModuleDropdownViewModel> GetCompanySetting()
		{
			var modules = ((CompanySettings[])Enum.GetValues(typeof(CompanySettings)))
			   .Select(c => new ModuleDropdownViewModel() { Id = (int)c, Name = c.ToString() }).ToList();
			return modules;
		}
		public List<Treatment> GetTreatments()
		{
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

			}
			var common = new Treatment()
			{
				Id = 0,
				Name = "-- Select --"
			};
			var treatments = _context.Treatments.Where(c => c.Id != 0 && c.CompanyBranchId == loggedInUser.CompanyBranchId && !c.Deleted)
				.Select(c => new Treatment()
				{
					Id = c.Id,
					Name = c.Name,
				}).ToList();
			treatments.Insert(0, common);
			return treatments;
		}
	

        public async Task<List<CommonDropDown>> GetDropdownByKey(DropdownEnums dropdownKey, bool deleteOption = false)
        {
            var dropdowns = await _context.CommonDropDowns.Where(s => !s.Deleted &&
                                s.DropdownKey == (int)dropdownKey).OrderBy(s => s.Name).ToListAsync();
            return dropdowns;
        }

        public List<ModuleDropdownViewModel> GetSpecies()
        {
            var data = new List<ModuleDropdownViewModel>();
            var species = ((Species[])Enum.GetValues(typeof(Species)))
               .Select(c => new ModuleDropdownViewModel() { Id = (int)c, Name = c.ToString() }).ToList();
            foreach (var item in species)
            {
                var enumId = (Species)item.Id;
                var description = subscriptionHelper.GetEnumDescription(enumId);
                var ddddata = new ModuleDropdownViewModel()
                {
                    Name = item.Name,
                    Id = item.Id,
                    NameToString = item.Name + " (" + description + ")"
                };
                data.Add(ddddata);
                
            }
            return data;
        }

        public List<Breed> GetBreeds(Species id)
        {
            var loggedInUser = Utitily.GetCurrentUser();
            if (loggedInUser.Id == null)
            {
                loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;
            }
            var breeds = new List<Breed>();
            if (loggedInUser != null)
            {
                breeds = _context.Breeds.Where(x => !x.Deleted && x.SpeciesId == id &&
                  ( x.CompanyBranchId == loggedInUser.CompanyBranchId || x.CompanyBranchId == null))?.ToList();
            }
            return breeds;
        }
		public List<Patient> GetPatients()
		{
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

			}
			var common = new Patient()
			{
				Id = 0,
				Name = "-- Select --"
			};
			var patients = _context.Patients.Where(c => c.Id != 0 && c.CompanyBranchId == loggedInUser.CompanyBranchId && c.Active)
				.Select(c => new Patient()
				{
					Id = c.Id,
					Name = c.Name,
				}).ToList();
			patients.Insert(0, common);
			return patients;
		}
	}
}
