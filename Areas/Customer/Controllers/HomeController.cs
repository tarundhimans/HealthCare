using E_Healthcare.Data;
using E_Healthcare.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;

namespace E_Healthcare.Areas.Customer.Controllers
{
    [Area("Customer")]

    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? page, string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                var doctor = _context.Doctors.FirstOrDefault(d => d.Name.Contains(search));
                if (doctor != null)
                {
                    if (!User.Identity.IsAuthenticated)
                    {
                        return Redirect("/Identity/Account/Login");
                    }
                    else
                    {
                        return RedirectToAction("DoctorDetails", new { id = doctor.Id });
                    }
                }
            }
            const int pageSize = 8;
            int pageNumber = page ?? 1;
            var totalDoctors = _context.Doctors.Count();
            var totalPages = (int)Math.Ceiling((double)totalDoctors / pageSize);
            if (pageNumber > totalPages)
            {
                pageNumber = totalPages;
            }
            var doctors = _context.Doctors.OrderBy(d => d.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = pageNumber;
            ViewBag.Search = search;
            return View(doctors);
        }       
        public IActionResult DoctorDetails(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Identity/Account/Login");
            }
            var doctor = _context.Doctors.FirstOrDefault(d => d.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }
            return View(doctor);
        }     

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
