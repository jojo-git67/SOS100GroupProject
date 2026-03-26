using System.ComponentModel.DataAnnotations;

namespace BookingAPI.Models;

public class Room
{
    [Key]
    public int RoomId { get; set; }

    public string RoomName { get; set; } = "";
    public int Capacity { get; set; }
}