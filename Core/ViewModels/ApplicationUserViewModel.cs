using static Core.Enums.VetManEnums;

namespace Core.ViewModels
{
	public class ApplicationUserViewModel
	{
		public string? Id { get; set; }
		public string? Email { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Password { get; set; }
		public string? PhoneNumber { get; set; }
		public string? ConfirmPassword { get; set; }
		public string? Address { get; set; }
		public Guid? CompanyBranchId { get; set; }
		public string?  CompanyBranch { get; set; }
		public string? DateRegistered { get; set; }
		public bool Delete { get; set; }
        public string FullName => FirstName + " " + LastName;
		public Guid? CompanyId { get; set; }
		public CompanySettings ModuleId { get; set; }
		public CompanySubcriptionStatus SubcriptionStatus { get; set; }
		public DateTime ExpiryDate { get; set; }
		public int StopDate
		{
			get
			{
				var daysLeft = (ExpiryDate - DateTime.Now).TotalDays;
				return Convert.ToInt32(daysLeft);
			}
		}
	}
}
