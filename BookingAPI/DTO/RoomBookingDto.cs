namespace BookingAPI.DTOs;

public class RoomBookingDto
{
    public int BookingId { get; set; }
    public int RoomId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int ActivityId { get; set; }
    public int BookedByUserId { get; set; }
    public string Status { get; set; } = "";
}