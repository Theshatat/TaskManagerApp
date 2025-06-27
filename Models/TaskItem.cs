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
        public int? CategoryId { get; set; } // Foreign key for Category
        public Category Category { get; set; } // Navigation property for Category
        public ApplicationUser User { get; set; }
    }
}
