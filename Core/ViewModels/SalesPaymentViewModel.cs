using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Enums.VetManEnums;

namespace Core.ViewModels
{
    public class SalesPaymentViewModel
    {
        public int OrderId { get; set; }
        public List<CommonDropDown>? Banks { get; set; }
        public int? SelectBank { get; set; }
        public double Amount { get; set; }
        public int SalesLogsId { get; set; }
        public int Id { get; set; }
        public double AmountPaid { get; set; }
        public double Balance { get; set; }
        public bool PaidCash { get; set; }
        public DateTime DateCreated { get; set; }
        public int SelectPayment { get; set; }
        
    }
}
