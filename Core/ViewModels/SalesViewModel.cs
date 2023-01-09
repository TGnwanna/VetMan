using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
	public class SalesViewModel
	{
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public int Id { get; set; }
        public double Discount { get; set; }
      
    }
}
