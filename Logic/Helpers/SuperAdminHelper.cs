using Core.Db;
using Core.Models;
using Core.ViewModels;
using Logic.IHelpers;

namespace Logic.Helpers
{
    public class SuperAdminHelper : ISuperAdminHelper
    {
        private readonly IEmailHelper _emailHelper;
        private readonly IUserHelper _userHelper;
        private readonly AppDbContext _context;
        public SuperAdminHelper(IUserHelper userHelper, AppDbContext context)
        {
            _userHelper = userHelper;
            _context = context;
        }




        public List<ModuleCostViewModel> GetModuleCostList()
        {
            var moduleCostList = new List<ModuleCostViewModel>();
            var loggedInUser = Utitily.GetCurrentUser();
            if (loggedInUser.Id == null)
            {
                loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName)?.Result;
            }

			if (loggedInUser != null)
			{
				moduleCostList = _context.ModuleCosts.Where(x => x.Active)
				.Select(s => new ModuleCostViewModel()
				{
					Id = s.Id,
					ModuleId = s.ModuleId,	
					Price = s.Price,
					Discription = s.Discription,
					NoOfDays = s.NoOfDays,
					Active = s.Active,
				})?.ToList();
				if (moduleCostList != null && moduleCostList.Count > 0)
				{
					return moduleCostList;
				}
			}
			return moduleCostList;
		}


		public bool CreateModuleCost(ModuleCostViewModel costDetails)
		{
			if (costDetails != null)
			{
				var costModel = new ModuleCost()
				{
					Price = costDetails.Price,
					ModuleId = costDetails.ModuleId,
					NoOfDays = costDetails.NoOfDays,
					Id = costDetails.Id,
					Discription = costDetails.Discription,
					Active = true,
				};
				_context.ModuleCosts.Add(costModel);
				_context.SaveChanges();
				return true;
			}
			return false;
		}


		public bool EditmoduleCost(ModuleCostViewModel costDetails)
		{
			if (costDetails != null)
			{
				var moduleCostToEdit = _context.ModuleCosts.Where(c => c.Id == costDetails.Id).FirstOrDefault();
				if (moduleCostToEdit != null)
				{
					moduleCostToEdit.Discription = costDetails.Discription;
					moduleCostToEdit.NoOfDays = costDetails.NoOfDays;
					moduleCostToEdit.Id = costDetails.Id;
					moduleCostToEdit.Price = costDetails.Price;
					moduleCostToEdit.ModuleId = costDetails.ModuleId;
					
				}
				_context.ModuleCosts.Update(moduleCostToEdit);
				_context.SaveChanges();
				return true;
			}
			return false;
		}


        public bool DeleteModuleCost(ModuleCostViewModel costDetails)
        {
            if (costDetails != null)         
			{
                var moduleCost = _context.ModuleCosts.Where(c => c.Id == costDetails.Id && c.Active).FirstOrDefault();
                if (moduleCost != null)
                {
					moduleCost.Active = false;
                }
                _context.ModuleCosts.Update(moduleCost);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

    }
}
