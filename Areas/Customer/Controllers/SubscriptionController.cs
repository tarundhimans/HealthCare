using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_Healthcare.Data;
using E_Healthcare.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace E_Healthcare.Controllers
{
    [Area("Customer")]
    public class SubscriptionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var subscriptions = await _context.Subscriptions
                .Include(s => s.Plan)
                .Include(s => s.ApplicationUser)
                .ToListAsync();

            return View(subscriptions);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions.Include(s => s.Plan).Include(s => s.ApplicationUser).FirstOrDefaultAsync(m => m.Id == id);
            if (subscription == null)
            {
                return NotFound();
            }

            return View(subscription);
        }
        public IActionResult Create()
        {
            ViewBag.PlanId = new SelectList(_context.SubscriptionPlans, "Id", "Name");
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            ViewBag.UserId = new SelectList(_context.Users.Where(u => u.Email == userEmail), "Id", "Email");
            var subscriptionPlans = _context.SubscriptionPlans.ToList();

            // Create a SelectList for the dropdown list
            ViewBag.SubscriptionPlans = new SelectList(subscriptionPlans, "Id", "Name");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubscriptionPlanId,ApplicationUserId,StartDate")] Subscription subscription)
        {           
                var selectedPlan = await _context.SubscriptionPlans.FindAsync(subscription.SubscriptionPlanId);
                if (selectedPlan != null)
                {
                    subscription.EndDate = subscription.StartDate.AddMonths(selectedPlan.DurationMonths);
                }

                _context.Add(subscription);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            

            ViewBag.PlanId = new SelectList(_context.SubscriptionPlans, "Id", "Name", subscription.SubscriptionPlanId);
            ViewBag.UserId = new SelectList(_context.Users, "Id", "Email", subscription.ApplicationUserId);
            return View(subscription);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }
            ViewBag.PlanId = new SelectList(_context.SubscriptionPlans, "Id", "Name", subscription.SubscriptionPlanId);
            ViewBag.UserId = new SelectList(_context.Users, "Id", "UserName", subscription.ApplicationUserId);
            return View(subscription);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SubscriptionPlanId,ApplicationUserId,StartDate,EndDate")] Subscription subscription)
        {
            if (id != subscription.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subscription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubscriptionExists(subscription.Id))
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
            ViewBag.PlanId = new SelectList(_context.SubscriptionPlans, "Id", "Name", subscription.SubscriptionPlanId);
            ViewBag.UserId = new SelectList(_context.Users, "Id", "UserName", subscription.ApplicationUserId);
            return View(subscription);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions
                .Include(s => s.Plan)
                .Include(s => s.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subscription == null)
            {
                return NotFound();
            }

            return View(subscription);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subscription = await _context.Subscriptions.FindAsync(id);
            _context.Subscriptions.Remove(subscription);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool SubscriptionExists(int id)
        {
            return _context.Subscriptions.Any(e => e.Id == id);
        }
    }
}
