using E_Healthcare.Data;
using E_Healthcare.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace E_Healthcare.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Doctor,Patient")]
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 8;

            var appointments = await _context.Appointments
                .Include(a => a.Doctor)
                .OrderByDescending(a => a.AppointmentDate)
                .ThenByDescending(a => a.AppointmentTime)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

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
        [Authorize(Roles = "Patient")]
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
            return RedirectToAction("Confirm", appointment);
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
        public IActionResult Confirm(Appointment appointment)
        {
            ViewBag.ConfirmationMessage = "Appointment confirmed successfully!";       
                       
            try
            {
                string smtpServer = "smtp-mail.outlook.com";
                int smtpPort = 587;
                string smtpUsername = "dhiman.cssoltuions@outlook.com";
                string smtpPassword = "Tgardens@12";

                // Create the email message
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(smtpUsername);
                mail.To.Add("dhiman.cssolutions@gmail.com");
                mail.Subject = "Appointment Confirmation";
                mail.Body = "Your appointment has been confirmed successfully!\n" +
                             "Name: " + appointment.Name + "\n" +
                             "Phone Number: " + appointment.PhoneNumber + "\n" +
                             "Personal Details: " + appointment.PersonalDetails + "\n" +
                             "Medical History: " + appointment.MedicalHistory + "\n" +
                             "Insurance Information: " + appointment.InsuranceInformation + "\n" +
                            "Appointment Date: " + appointment.AppointmentDate.ToString("MM/dd/yyyy") + "\n" +
                            "Appointment Time: " + appointment.AppointmentTime.ToString(@"hh\:mm") ;


                SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = true;

                // Send the email
                smtpClient.Send(mail);

                ViewBag.Message = "Email sent successfully!";
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred: " + ex.Message;
            }

            return View(appointment);
        }
    }
}