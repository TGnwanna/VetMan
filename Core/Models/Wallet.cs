namespace Core.Models
{
	public class Wallet : BaseModel
	{
		public new Guid Id { get; set; }
		public double Balance { get; set; }
	}
}
