using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class PaymentMeans : BaseModel
    {
        public int CommonDropDownId { get; set; }
        [ForeignKey("CommonDropDownId")]
        public virtual CommonDropDown? CommonDropDown { get; set; }
    }
}
