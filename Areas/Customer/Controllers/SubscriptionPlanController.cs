using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_Healthcare.Data;
using E_Healthcare.Models;
using System.Net.Mail;
using System.Net;

namespace E_Healthcare.Controllers
{
    [Area("Customer")]
    public class SubscriptionPlanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionPlanController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var subscriptionPlans = await _context.SubscriptionPlans.ToListAsync();
            return View(subscriptionPlans);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var subscriptionPlan = await _context.SubscriptionPlans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subscriptionPlan == null)
            {
                return NotFound();
            }
            return View(subscriptionPlan);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,DurationMonths")] SubscriptionPlan subscriptionPlan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subscriptionPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subscriptionPlan);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var subscriptionPlan = await _context.SubscriptionPlans.FindAsync(id);
            if (subscriptionPlan == null)
            {
                return NotFound();
            }
            return View(subscriptionPlan);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,DurationMonths")] SubscriptionPlan subscriptionPlan)
        {
            if (id != subscriptionPlan.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subscriptionPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubscriptionPlanExists(subscriptionPlan.Id))
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
            return View(subscriptionPlan);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var subscriptionPlan = await _context.SubscriptionPlans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subscriptionPlan == null)
            {
                return NotFound();
            }
            return View(subscriptionPlan);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subscriptionPlan = await _context.SubscriptionPlans.FindAsync(id);
            _context.SubscriptionPlans.Remove(subscriptionPlan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool SubscriptionPlanExists(int id)
        {
            return _context.SubscriptionPlans.Any(e => e.Id == id);
        }
        public async Task<IActionResult> Subscribe(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                // Fetch subscription plan details based on the provided ID
                var subscriptionPlan = await _context.SubscriptionPlans.FindAsync(id);

                if (subscriptionPlan == null)
                {
                    return NotFound();
                }

                // Your email configuration settings
                string smtpServer = "smtp-mail.outlook.com";
                int smtpPort = 587;
                string smtpUsername = "dhiman.cssoltuions@outlook.com";
                string smtpPassword = "Tgardens@12";

                // Create the email message
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(smtpUsername);
                mail.To.Add("dhiman.cssolutions@gmail.com"); // Change this to the subscriber's email address
                mail.Subject = "Subscription Confirmation";
                mail.Body = "Thank you for subscribing. Your purchase was successful!";

                // Configure SMTP client
                SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = true;

                // Send the email
                smtpClient.Send(mail);

                // Optionally, you can redirect to a success page after sending the email
                return View();
            }
            catch (Exception ex)
            {
                // Handle any errors occurred during email sending
                ViewBag.Error = "An error occurred: " + ex.Message;
                return View("Error");
            }
        }
    }

}
