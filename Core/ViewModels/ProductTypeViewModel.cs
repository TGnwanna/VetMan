namespace Core.ViewModels
{
    public class ProductTypeViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
