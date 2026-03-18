namespace BookingMVC.Models;

public class RoomBookingPageViewModel
{
    public List<RoomDto> Rooms { get; set; } = new();
    public List<RoomBookingDto> Bookings { get; set; } = new();
}