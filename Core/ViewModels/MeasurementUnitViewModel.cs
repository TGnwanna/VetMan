using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
	public class MeasurementUnitViewModel
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public bool Active { get; set; }
		public bool Deleted { get; set; }
		public DateTime DateCreated { get; set; }
		public Guid? CompanyBranchId { get; set; }
		
	}
}
