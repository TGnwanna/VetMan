using static Core.Enums.VetManEnums;

namespace Core.ViewModels
{
	public class ModuleCostViewModel
	{
		public Guid Id { get; set; }
		public CompanySettings ModuleId { get; set; }
		public Guid CompanyModuleId { get; set; }
		public double Price { get; set; }
		public int NoOfDays { get; set; }
		public string? Discription { get; set; }
		public bool Active { get; set; }


	}

	public class ModulesData
	{
		public virtual List<ModuleCostViewModel> ModuleCost { get; set; }
		public virtual List<CompanyModuleViewModel> Plans { get; set; }
	}
}
