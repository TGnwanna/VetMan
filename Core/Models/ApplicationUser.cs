using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? Address { get; set; }
        [NotMapped]
        public string FullName => FirstName + " " + LastName;
        public Guid? CompanyBranchId { get; set; }
        [ForeignKey("CompanyBranchId")]
        public virtual CompanyBranch? CompanyBranch { get; set; }
        public DateTime DateRegistered { get; set; }
        //  public bool IsAdmin { get; set; }

        public bool IsDeactivated { get; set; }
        [NotMapped]
        public List<string> Roles { get; set; }
        [NotMapped]
        public string UserRole { get; set; }
        [NotMapped]
        public string BranchName { get; set; }
        [NotMapped]
        public string? BaseUrl { get; set; }
    }
}
