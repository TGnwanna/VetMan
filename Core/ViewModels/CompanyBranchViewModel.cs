using Core.Models;

namespace Core.ViewModels
{
    public class CompanyBranchViewModel
    {
        public Guid Id { get; set; }

        public Guid? CompanyId { get; set; }
       
        public string? Name { get; set; }

        public string? Address { get; set; }

        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
