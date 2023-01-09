﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class UserVerification
    {
        [Key]
        public Guid? Token { get; set; }
        public string? UserId { get; set; }
        public bool Used { get; set; }
        public DateTime? DateUsed { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }
    }
}
