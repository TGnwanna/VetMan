using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class CommonDropDown : BaseModel
	{
		[Display(Name = "Dropdown Key")]
		public int DropdownKey { get; set; }
	}
}
