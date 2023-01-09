using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class PaymentMeansViewModel
    {
        public string? Name { get; set; }
        public int CategoryId { get; set; }
        public int SalesLogId { get; set; }
        public double Amount { get; set; }
    }
}
