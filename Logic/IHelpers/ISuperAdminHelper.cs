using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.IHelpers
{
	public interface ISuperAdminHelper
	{
		List<ModuleCostViewModel> GetModuleCostList();
		bool CreateModuleCost(ModuleCostViewModel costDetails);
		bool EditmoduleCost(ModuleCostViewModel costDetails);
		bool DeleteModuleCost(ModuleCostViewModel costDetails);

	}
}
