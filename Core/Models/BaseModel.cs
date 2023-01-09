using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
	public class BaseModel
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public bool Active { get; set; }
		public bool Deleted { get; set; }
		public DateTime DateCreated { get; set; }
		public Guid? CompanyBranchId { get; set; }
		[ForeignKey("CompanyBranchId")]
		public virtual CompanyBranch? CompanyBranch { get; set; }
	}
}
