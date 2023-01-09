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
    public class Visit : BaseModel
    {
        [Required]
        public int? PatientId { get; set; }
        [Display(Name = "Patient")]
        [ForeignKey("PatientId")]
        public virtual Patient? Patient { get; set; }

        public string? TreatmentHistory { get; set; }
        public string? VaccinationHistory { get; set; }
        public string? EnvironmentalHistory { get; set; }
        public string? FeedingHistory { get; set; }
        public string? GeneralExamination { get; set; }
        public string? PhysicalExamination { get; set; }
        public string? PhysiologicalExamination { get; set; }
        public string? LaboratoryExamination { get; set; }
        public string? LaboratorySamples { get; set; }
        public string? LaboratoryResults { get; set; }
        public string? DifferentialDiagnosis { get; set; }
        [Required]
        public string? DefinitiveDiagnosis { get; set; }
        public string? Treatment { get; set; }

        [Required]
        public string? CreatedById { get; set; }
        [Display(Name = "CreatedBy")]
        [ForeignKey("CreatedById")]
        public virtual ApplicationUser? CreatedBy { get; set; }

        [Required]
        public string? PrimaryComplaint { get; set; }
    }
}
