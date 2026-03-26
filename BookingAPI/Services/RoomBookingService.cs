using BookingAPI.Data;
using BookingAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BookingAPI.Services;

public class RoomBookingService : IRoomBookingService
{
    private readonly BookingDbContext _context;

    public RoomBookingService(BookingDbContext context)
    {
        _context = context;
    }

    public async Task<List<RoomBookingDto>> GetAllBookingsAsync()
    {
        var bookings = await _context.RoomBookings.ToListAsync();

        return bookings.Select(b => new RoomBookingDto
        {
            BookingId = b.BookingId,
            RoomId = b.RoomId,
            Date = b.Date,
            StartTime = b.StartTime,
            EndTime = b.EndTime,
            ActivityId = b.ActivityId,
            BookedByUserId = b.BookedByUserId,
            Status = b.Status
        }).ToList();
    }

    public async Task<RoomBookingDto?> GetBookingByIdAsync(int id)
    {
        var booking = await _context.RoomBookings.FindAsync(id);

        if (booking == null)
        {
            return null;
        }

        return new RoomBookingDto
        {
            BookingId = booking.BookingId,
            RoomId = booking.RoomId,
            Date = booking.Date,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            ActivityId = booking.ActivityId,
            BookedByUserId = booking.BookedByUserId,
            Status = booking.Status
        };
    }
}