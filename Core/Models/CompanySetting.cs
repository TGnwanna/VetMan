using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class CompanySetting
    {
        [Key]
        public Guid? CompanyId { get; set; }
        [Display(Name = "Company")]
        [ForeignKey("CompanyId")]
        public virtual Company? Company { get; set; }
        [Display(Name = "DOC Booking Module")]
        public bool DOCBookingModule { get; set; }

        [Display(Name = "Vaccine Module")]
        public bool VaccineModule { get; set; }

        [Display(Name = "Transaction Module")]
        public bool TransactionModule { get; set; }

        [Display(Name = "StoreModule Module")]
        public bool StoreModule { get; set; }
    }
}
