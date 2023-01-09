using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Core.Models
{
	public class Customer : BaseModel
	{
		public new Guid Id { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Email { get; set; }
		public string? Address { get; set; }
		public string? Profession { get; set; }
		public bool IsDeleted { get; set; }
        public int? GenderId { get; set; }
        [Display(Name = "Gender")]
        [ForeignKey("GenderId")]
        public virtual CommonDropDown? Gender { get; set; }

        public string? PhoneNumber { get; set; }
		[NotMapped]
		public string FullName => FirstName + " " + LastName;

	}
}
