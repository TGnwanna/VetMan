using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static Core.Enums.VetManEnums;
using System.Xml.Linq;

namespace Core.ViewModels
{
	public class CompanyModuleViewModel
	{
        public Guid Id { get; set; }
        public CompanySettings ModuleId { get; set; }
        public CompanySubcriptionStatus SubcriptionStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public Guid? ModuleCostId { get; set; }
        public int Duration { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }

    }
}
