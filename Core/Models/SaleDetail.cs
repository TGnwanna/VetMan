using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class SaleDetail
    {
        public int Id { get; set; }

        public int InventoryId { get; set; }
        [ForeignKey("InventoryId")]
        public virtual ProductInventory Inventory  { get; set; }

        public int SalesLogId { get; set; }
        [ForeignKey("SalesLogId")]
        public virtual SalesLog SalesLog { get; set; }

        public int Quantity { get; set; }

        public double SellingPrice { get; set; }
        public double Discount { get; set; }
      
    }
}
