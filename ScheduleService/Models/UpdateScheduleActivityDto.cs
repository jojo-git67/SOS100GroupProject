namespace ScheduleService.Models
{
    public class UpdateScheduleActivityDto
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int RoomId { get; set; }
    }
}