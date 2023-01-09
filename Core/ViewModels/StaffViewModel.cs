using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
	public class StaffViewModel
	{
		public string? Name { get; set; }
		public string? RoleName { get; set; }
		public string? Id { get; set; }
		public Guid? BranchId { get; set; }

		public List<DropdownEnumModel> RoleAccess { get; set; }
	}
}
