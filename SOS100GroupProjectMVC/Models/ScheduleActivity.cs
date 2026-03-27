namespace SOS100GroupProjectMVC.Models
{
    public class ScheduleActivity
    {
        public int ActivityId { get; set; }
        public int CourseId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int RoomId { get; set; }
    }
}