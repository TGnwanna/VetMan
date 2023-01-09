using static Core.Enums.VetManEnums;

namespace Core.Models
{
	public class ModuleCost
	{
		public Guid Id { get; set; }
		public double Price { get; set; }
		public int NoOfDays { get; set; }
		public string? Discription { get; set; }
		public CompanySettings ModuleId { get; set; }
		public bool Active { get; set; }
	}
}
