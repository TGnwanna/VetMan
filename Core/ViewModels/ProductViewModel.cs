using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Enums.VetManEnums;

namespace Core.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public DateTime? DateCreated { get; set; }
        public string? ProductTypeName { get; set; }
        public int? ProductTypeId { get; set; }
        public string? Name { get; set; }
        public bool Active { get; set; }
    }
}
