namespace SOS100GroupProjectMVC.Models;

public class RoomBookingPageViewModel
{
    public List<RoomDto> Rooms { get; set; } = new();
    public List<RoomBookingDto> Bookings { get; set; } = new();

    public RoomBookingDto NewBooking { get; set; } = new()
    {
        Date = DateTime.Today,
        StartTime = new TimeSpan(9, 0, 0),
        EndTime = new TimeSpan(10, 0, 0),
        Status = "Created"
    };
}