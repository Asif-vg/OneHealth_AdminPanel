using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OneHealth.Models
{
    public class Setting
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(250)]
        public string Adddress { get; set; }
        [MaxLength(20)]
        public string Phone1 { get; set; }
        [MaxLength(50)]
        public string Email1 { get; set; }
        [MaxLength(20)]
        public string Phone2 { get; set; }
        [MaxLength(50)]
        public string Email2 { get; set; }
        [MaxLength(250)]
        public string Logo { get; set; }
        [NotMapped]
        public string LogoFile { get; set; }
        [MaxLength(2000)]
        public string About { get; set; }


    }
}
