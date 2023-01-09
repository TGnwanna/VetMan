using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Core.Enums.VetManEnums;

namespace Core.Models
{
    public class Patient : BaseModel
    {
        public DateTime DOB { get; set; }
        public Species SpeciesId { get; set; }
        public int GenderId { get; set; }
        [Display(Name = "Gender")]
        [ForeignKey("GenderId")]
        public virtual CommonDropDown? Gender { get; set; }

        public int? BreedId { get; set; }
        [Display(Name = "Breed")]
        [ForeignKey("BreedId")]
        public virtual Breed? Breed { get; set; }

        public string? Colour { get; set; }
        [Display(Name = "Special Marking")]
        public string? SpecialMarking { get; set; }
        public string? Picture { get; set; }
        public string? CaseNumber { get; set; }

        public Guid? CustomerId { get; set; }
        [Display(Name = "Customer")]
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }
        
    }
}
