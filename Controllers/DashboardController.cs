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
                //CompletedTasks = allTasks.Count(t => t.Status == "Completed"), this is case-sensitive
                CompletedTasks = allTasks.Count(t =>
                    string.Equals(t.Status, "Completed", StringComparison.OrdinalIgnoreCase)),
                PendingTasks = allTasks.Count(t =>
                    string.Equals(t.Status, "Pending", StringComparison.OrdinalIgnoreCase)),
                InProgressTasks = allTasks.Count(t =>
                    string.Equals(t.Status, "In Progress", StringComparison.OrdinalIgnoreCase))
            };
            // Tasks by Category
            var categoryGroups = allTasks
                .GroupBy(t => t.Category?.Name ?? "Uncategorized")
                .ToList();

            foreach (var group in categoryGroups)
            {
                dashboardViewModel.Categories.Add(group.Key);
                dashboardViewModel.TasksByCategory.Add(group.Count());
            }

            // Tasks by Status
            var statusGroups = allTasks
                .GroupBy(t => t.Status)
                .ToList();

            foreach (var group in statusGroups)
            {
                dashboardViewModel.Statuses.Add(group.Key);
                dashboardViewModel.TasksByStatus.Add(group.Count());
            }

            return View(dashboardViewModel);
        }
    }
}
