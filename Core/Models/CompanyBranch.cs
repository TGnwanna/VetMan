using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class CompanyBranch
    {
        public Guid Id { get; set; }
        public Guid? CompanyId { get; set; }
        [Display(Name = "Company")]
        [ForeignKey("CompanyId")]
        public virtual Company? Company { get; set; }

        [Display(Name = "Branch Name")]
        public string Name { get; set; }

        public string Address { get; set; }

        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public bool IsMainBranch { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
