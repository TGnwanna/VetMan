using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class SaleLogsViewModel
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid? CustomerId { get; set; }
        public string? UserId { get; set; }
        public string? CustomerName { get; set; }
        public string? SaleDetails { get; set; }
        public virtual List<SalesDetailViewModel>? SaleDetailsModel { get; set; }
        public string? Staff { get; set; }
        public string? Customer { get; set; }
        public string? CustomerAddress { get; set; }
        public double TotalAmountPaid { get; set; }
        public string? DateFromD { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo{ get; set; }
        public string? Phone { get; set; }

    }
}
