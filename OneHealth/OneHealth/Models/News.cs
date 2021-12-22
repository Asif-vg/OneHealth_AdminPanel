using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OneHealth.Models
{
    public class News
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(250)]
        public string Title { get; set; }
        [MaxLength(250)]
        public string NewsImage { get; set; }
        [NotMapped]
        public IFormFile NewsImageFile { get; set; }
        public DateTime CreatedDate { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
        [ForeignKey("NewsCategory")]
        public int CategoryId { get; set; }
        public NewsCategory NewsCategory { get; set; }

        public List<TagToNews> TagToNews { get; set; }

        [NotMapped]
        public List<int> TagToNewsId { get; set; }



    }
}
