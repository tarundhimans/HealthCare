using E_Healthcare.Data;
using E_Healthcare.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace E_Healthcare.Controllers
{
    [Area("Customer")]
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var appointments = await _context.Appointments.Include(a => a.Doctor).ToListAsync();
            return View(appointments);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var patient = await _context.Appointments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }
        public IActionResult Create()
        {
            ViewBag.Doctors = _context.Doctors.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
     
        public async Task<IActionResult> Create([Bind("Id,Name,PhoneNumber,PersonalDetails,MedicalHistory,InsuranceInformation,DoctorId")] Appointment appointment)
        {
          
                var doctorExists = await _context.Doctors.AnyAsync(d => d.Id == appointment.DoctorId);
                if (!doctorExists)
                {
                    ModelState.AddModelError("DoctorId", "Invalid DoctorId. Please select a valid doctor.");
                    ViewBag.Doctors = _context.Doctors.ToList(); 
                    return View(appointment);
                }

                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));          
           
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewBag.Doctors = await _context.Doctors.ToListAsync();
            return View(appointment);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,PhoneNumber,PersonalDetails,MedicalHistory,InsuranceInformation,DoctorId")] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }          
            ViewBag.Doctors = await _context.Doctors.ToListAsync();
            return View(appointment);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var patient = await _context.Appointments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Appointments.FindAsync(id);
            _context.Appointments.Remove(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}