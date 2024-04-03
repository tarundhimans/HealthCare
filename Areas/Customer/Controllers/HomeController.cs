using E_Healthcare.Data;
using E_Healthcare.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace E_Healthcare.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index(int? page)
        {
            const int pageSize = 8;
            int pageNumber = page ?? 1;

            var totalDoctors = _context.Doctors.Count();
            var totalPages = (int)Math.Ceiling((double)totalDoctors / pageSize);

            if (pageNumber > totalPages)
            {
                pageNumber = totalPages;
            }

            var doctors = _context.Doctors
                .OrderBy(d => d.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = pageNumber;

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
