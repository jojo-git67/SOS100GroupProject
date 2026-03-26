using BookingAPI.DTOs;

namespace BookingAPI.Services;

public interface IRoomBookingService
{
    Task<List<RoomBookingDto>> GetAllBookingsAsync();
    Task<RoomBookingDto?> GetBookingByIdAsync(int id);
}