using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ScreenRole : BaseModel
    {
        public string? RoleId { get; set; }
        public int ScreenId { get; set; }
        public bool ScreenChecked { get; set; }
        public DateTime DateAssigned { get; set; }
    }
}
