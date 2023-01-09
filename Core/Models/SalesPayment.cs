using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Enums.VetManEnums;

namespace Core.Models
{
    public class SalesPayment 
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public double AmountPaid { get; set; }
        public double Balance { get; set; }
        public DropdownEnums DepositType { get; set; }
        public DateTime PaymentDate { get; set; }
        public int SalesLogsId { get; set; }
        [ForeignKey("SalesLogsId")]
        public virtual SalesLog? SalesLog { get; set; }
        public int PaymentMeansId { get; set; }
        [ForeignKey("PaymentMeansId")]
        public virtual PaymentMeans? PaymentMeans { get; set; }

        public Guid? CompanyBranchId { get; set; }
        [ForeignKey("CompanyBranchId")]
        public virtual CompanyBranch? CompanyBranch { get; set; }
    }
}
