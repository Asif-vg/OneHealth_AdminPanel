using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OneHealth.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Fullname { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(100)]
        public string Subject { get; set; }
        [MaxLength(4000)]
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
