using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneHealth.Data;
using OneHealth.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneHealth.Controllers
{
    public class DoctorController : Controller
    {
        private readonly AppDbContext _context;

        public DoctorController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            VmDoctor model = new VmDoctor()
            {
                 Doctor= _context.Doctors.Include(p => p.Position).ToList()
            };
            return View(model);
        }
    }
}
