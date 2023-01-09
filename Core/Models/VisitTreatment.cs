using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core.Models
{
    public class VisitTreatment
    {
        public Guid Id { get; set; }
        public double? Discount { get; set; }
        public double? Cost { get; set; }
        public int? TreatmentId { get; set; }
        [Display(Name = "Treatment")]
        [ForeignKey("TreatmentId")]
        public virtual Treatment? Treatment { get; set; }

        public int? VisitId { get; set; }
        [Display(Name = "Visit")]
        [ForeignKey("VisitId")]
        public virtual Visit? Visit { get; set; }
        public string? CreatedById { get; set; }
        [Display(Name = "CreatedBy")]
        [ForeignKey("CreatedById")]
        public virtual ApplicationUser? CreatedBy { get; set; }
        public Guid? CompanyBranchId { get; set; }
        [ForeignKey("CompanyBranchId")]
        public virtual CompanyBranch? CompanyBranch { get; set; }
    }
}
