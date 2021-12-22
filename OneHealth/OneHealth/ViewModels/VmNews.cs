using OneHealth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneHealth.ViewModels
{
    public class VmNews
    {
        public List<News> News { get; set; }
        public List<NewsCategory> NewsCategories { get; set; }
        public Tag Tag { get; set; }


    }
}
