using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Impersonation
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Impersonator Id")]
        public string SuperAdminUserId { get; set; }


        [Display(Name = "Impersonate Id")]
        public string CompanyAdminId { get; set; }


        [Display(Name = "Date Impersonated")]
        public DateTime DateImpersonated { get; set; }


        [Display(Name = "Is Impersonation session Ended")]
        public bool EndSession { get; set; }

        [Display(Name = "Date Impersonation Session Ended")]
        public DateTime DateSessionEnded { get; set; }


        [Display(Name = "Is checking if session is on and logs in the real user")]
        public bool AmTheRealUser { get; set; }
    }
}
