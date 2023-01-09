namespace Core.ViewModels
{
    public class CompanyViewModel
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public string CompanyAddress { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string? CreatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public string FirstName { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string AdminFullName { get; set; }
        public bool IsAdmin { get; set; }
    }
}
