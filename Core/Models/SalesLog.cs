using System.ComponentModel.DataAnnotations.Schema;
using static Core.Enums.VetManEnums;

namespace Core.Models
{
	public class SalesLog : BaseModel
	{
		public string? SalesDetail { get; set; }

		public double TotalPaid { get; set; }

		public Guid? CustomerId { get; set; }
		[ForeignKey("CustomerId")]
		public virtual Customer? Customer { get; set; }

		public string? UserId { get; set; }
		[ForeignKey("UserId")]
		public virtual ApplicationUser? User { get; set; }
		public DropdownEnums DepositType { get; set; }
	}
}
