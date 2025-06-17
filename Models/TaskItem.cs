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
        public ApplicationUser User { get; set; }
    }
}
