using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Product : BaseModel
    {
        public int? ProductTypeId { get; set; }
        [ForeignKey("ProductTypeId")]
        public virtual ProductType? ProductType { get; set; }
    }
}
