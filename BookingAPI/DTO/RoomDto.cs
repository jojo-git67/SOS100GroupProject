namespace BookingAPI.DTOs;

public class RoomDto
{
    public int RoomId { get; set; }
    public string RoomName { get; set; } = "";
    public int Capacity { get; set; }
}