using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Company
    {
        [Key]
        public Guid Id { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "Company Email")]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Display(Name = "Mobile Number")]
        public string Mobile { get; set; }

        public string? CreatedById { get; set; }
        [Display(Name = "CreatedBy")]
        [ForeignKey("CreatedById")]
        public virtual ApplicationUser? CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string? Name { get; set; }

        public bool Deleted { get; set; }
        public bool Active { get; set; }
    }
}
