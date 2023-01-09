using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core.ViewModels
{
    public class VisitViewModel
    {
        public string? PatientPicture { get; set; }
        public string? PatientName { get; set; }
        public int? PatientId { get; set; }
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
        public string? DefinitiveDiagnosis { get; set; }
        public string? CreatedById { get; set; }
        public string? CreatedName { get; set; }
        public string? PrimaryComplaint { get; set; }
        public string? Purpose { get; set; }
        public int Id { get; set; }
        public string? Breed { get; set; }
        public string? CaseNumber { get; set; }
        public DateTime NextDate { get; set; }
        public Guid? CompanyBranchId { get; set; }
        public bool SMSAllowed { get; set; }
        public bool EmailAllowed { get; set; }
        
    }
}
