using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Treatment : BaseModel
    {
        public double Price { get; set; }
        public string Description { get; set; }
    }
}
