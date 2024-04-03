    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using E_Healthcare.Data;
    using E_Healthcare.Models;
    using System.Threading.Tasks;
    using System.Security.Claims;

    namespace E_Healthcare.Controllers
    {
        [Area("Admin")]
        public class DoctorController : Controller
        {
            private readonly ApplicationDbContext _context;

            public DoctorController(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<IActionResult> Index()
            {
                var doctors = await _context.Doctors.ToListAsync();
                return View(doctors);
            }
            public async Task<IActionResult> Details(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }
                var doctor = await _context.Doctors.FirstOrDefaultAsync(m => m.Id == id);
                if (doctor == null)
                {
                    return NotFound();
                }
                return View(doctor);
            }
            public IActionResult Create()
            {
                return View();
            }      
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create([Bind("Id,Name,ApplicationUserId,Specialties,Qualifications,ConsultationFee")] Doctor doctor)
            {

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);               
                    doctor.ApplicationUserId = userId;
                    _context.Add(doctor);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
            
            }
            public async Task<IActionResult> Edit(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var doctor = await _context.Doctors.FindAsync(id);
                if (doctor == null)
                {
                    return NotFound();
                }
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (doctor.ApplicationUserId != userId)
                {
                    return Unauthorized(); 
                }
                return View(doctor);
            }
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Specialties,Qualifications,ConsultationFee")] Doctor doctor)
            {
                if (id != doctor.Id)
                {
                    return NotFound();
                }

                var existingDoctor = await _context.Doctors.FindAsync(id);
                if (existingDoctor == null)
                {
                    return NotFound();
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (existingDoctor.ApplicationUserId != userId)
                {
                    return Unauthorized();
                }

                    try
                    {
                   
                        existingDoctor.Specialties = doctor.Specialties;
                        existingDoctor.Qualifications = doctor.Qualifications;
                        existingDoctor.ConsultationFee = doctor.ConsultationFee;

                        _context.Update(existingDoctor);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!DoctorExists(doctor.Id))
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


            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }
                var doctor = await _context.Doctors.FirstOrDefaultAsync(m => m.Id == id);
                if (doctor == null)
                {
                    return NotFound();
                }
                return View(doctor);
            }
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var doctor = await _context.Doctors.FindAsync(id);
                _context.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            private bool DoctorExists(int id)
            {
                return _context.Doctors.Any(e => e.Id == id);
            }
        }
    }
