using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Enums.VetManEnums;

namespace Core.Models
{
    public class ProductLog : BaseModel
    {
        public int ProductInventoryId { get; set; }
        [ForeignKey("ProductInventoryId")]
        public virtual ProductInventory ProductInventory { get; set; }

        public int Qauntity { get; set; }
        public int OldQauntity { get; set; }
        public int NewQuantity { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public ProductActivity ProductActivity { get; set; }
        public double SellingPrice { get; set; }
    }
}
