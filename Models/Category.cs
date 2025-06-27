namespace TaskManagerApp.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        public ICollection<TaskItem> TaskItems { get; set; } // Navigation property for related tasks
    }
}
