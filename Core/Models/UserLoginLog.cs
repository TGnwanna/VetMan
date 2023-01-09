using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class UserLoginLog
    {
        public Guid Id { get; set; }

        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }   

        public DateTime LoginDate { get; set; }
        public string? IPAddress { get; set; }
        public string? DeviceName { get; set; } 
    }
}
