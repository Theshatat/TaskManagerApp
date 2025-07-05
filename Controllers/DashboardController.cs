using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Data;

namespace TaskManagerApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the current user's ID from claims
            // Alternatively, you can use the commented line below if you prefer to fetch it from the database
            /*string userId = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name)?.Id;*/ // Get the current user's ID
            var allTasks = await _context.TaskItems
                .Where(t => t.UserId == userId)
                .ToListAsync();
            var dashboardViewModel = new DashboardViewModel
            {
                TotalTasks = allTasks.Count,
                CompletedTasks = allTasks.Count(t => t.Status == "Completed"),
                PendingTasks = allTasks.Count(t => t.Status == "Pending"),
                InProgressTasks = allTasks.Count(t => t.Status == "In Progress")
            };

            return View();
        }
    }
}
