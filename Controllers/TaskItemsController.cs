using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Data;
using TaskManagerApp.Models;

namespace TaskManagerApp.Controllers
{
    public class TaskItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaskItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TaskItems
        public async Task<IActionResult> Index(int? categoryId,
                            string status,
                            string searchString,
                            DateTime? dueDateStart,
                            DateTime? dueDateEnd)
        {
            IQueryable<TaskItem> tasksQuery = _context.TaskItems.Include(t => t.User).Include(t => t.Category);
            // Get all categories to populate the dropdown
            var categories = await _context.Categories
                .Where(c => c.Name != null) // filter to avoid null names
                .ToListAsync();

            // Apply filter if category is selected
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                tasksQuery = tasksQuery.Where(t => t.CategoryId == categoryId.Value);
            }
            // Apply Status Filter
            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                tasksQuery= tasksQuery.Where(t => t.Status == status);
            }

            // Apply Keyword Search
            if (!string.IsNullOrEmpty(searchString))
            {
                tasksQuery= tasksQuery.Where(t =>
                    t.Name.Contains(searchString) ||
                    t.Description.Contains(searchString));
            }

            // Apply Due Date Range
            if (dueDateStart.HasValue)
            {
                tasksQuery= tasksQuery.Where(t => t.DueDate >= dueDateStart.Value.Date);
            }
            if (dueDateEnd.HasValue)
            {
                tasksQuery= tasksQuery.Where(t => t.DueDate <= dueDateEnd.Value.Date.AddDays(1).AddSeconds(-1)); 
            }
            // Create a view model or use ViewBag to pass categories
            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name", categoryId);
            ViewBag.SelectedCategory = categoryId ?? 0;
            ViewBag.StatusOptions = new SelectList(StatusOptions);
            ViewBag.CurrentStatus = status;
            ViewBag.CurrentSearch = searchString;
            ViewBag.CurrentStartDate = dueDateStart?.ToString("yyyy-MM-dd");
            ViewBag.CurrentEndDate = dueDateEnd?.ToString("yyyy-MM-dd");

            var model = await tasksQuery.ToListAsync();

            return View(model);
        }
        // GET: TaskItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.TaskItems
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskItem == null)
            {
                return NotFound();
            }

            return View(taskItem);
        }

        // GET: TaskItems/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: TaskItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,DueDate,Status,UserId")] TaskItem taskItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taskItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", taskItem.UserId);
            return View(taskItem);
        }

        // GET: TaskItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", taskItem.UserId);
            return View(taskItem);
        }

        // POST: TaskItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,DueDate,Status,UserId")] TaskItem taskItem)
        {
            if (id != taskItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskItemExists(taskItem.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", taskItem.UserId);
            return View(taskItem);
        }

        // GET: TaskItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.TaskItems
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskItem == null)
            {
                return NotFound();
            }

            return View(taskItem);
        }

        // POST: TaskItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem != null)
            {
                _context.TaskItems.Remove(taskItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskItemExists(int id)
        {
            return _context.TaskItems.Any(e => e.Id == id);
        }

        private readonly List<string> StatusOptions = new List<string>
        {
            "All",
            "Pending",
            "In Progress",
            "Completed"
        };
    }
}
