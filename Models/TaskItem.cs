using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TaskManagerApp.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; } // e.g., "Pending", "In Progress", "Completed"
        public string UserId { get; set; }
        [Display(Name = "Category")]
        public int? CategoryId { get; set; } // Foreign key for Category
        [BindNever]
        public Category Category { get; set; } // Navigation property for Category
        [BindNever]
        public ApplicationUser User { get; set; }
    }
}
