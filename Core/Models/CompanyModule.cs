using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Core.Enums.VetManEnums;

namespace Core.Models
{
    public class CompanyModule
    {
        public Guid Id { get; set; }
        public Guid? CompanyId { get; set; }
        [Display(Name = "Company")]
        [ForeignKey("CompanyId")]
        public virtual Company? Company { get; set; }
        public CompanySettings ModuleId { get; set; }
        public CompanySubcriptionStatus SubcriptionStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public Guid? ModuleCostId { get; set; }
        [ForeignKey("ModuleCostId")]
        public virtual ModuleCost? ModuleCost { get; set; }
    }
}
