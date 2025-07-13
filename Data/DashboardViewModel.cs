namespace TaskManagerApp.Data
{
    public class DashboardViewModel
    {
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int PendingTasks { get; set; }
        public int InProgressTasks { get; set; }
        // Chart Data
        public List<string> Categories { get; set; } = new List<string>();
        public List<int> TasksByCategory { get; set; } = new List<int>();

        public List<string> Statuses { get; set; } = new List<string>();
        public List<int> TasksByStatus { get; set; } = new List<int>();

    }
}
