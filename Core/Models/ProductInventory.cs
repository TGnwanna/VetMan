using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
	public class ProductInventory : BaseModel
    {
        public int? SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier? Supplier { get; set; }


        public int? MeasurementunitId { get; set; }
        [ForeignKey("MeasurementunitId")]
        public virtual Measurementunit? Measurementunit { get; set; }

        public int? ShopCategoryId { get; set; }
        [ForeignKey("ShopCategoryId")]
        public virtual ShopCategory? ShopCategory { get; set; }
        public int Quantity { get; set; }
        public double Amount { get; set; }
        public double AmountPerProduct { get; set; }
        
    }
}
