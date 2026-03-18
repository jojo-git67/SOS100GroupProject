using BookingAPI.Data;
using BookingAPI.Models;
using BookingAPI.DTOs;
using BookingAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomBookingsController : ControllerBase
{
    private readonly BookingDbContext _context;
    private readonly IRoomBookingService _roomBookingService;

    public RoomBookingsController(BookingDbContext context, IRoomBookingService roomBookingService)
    {
        _context = context;
        _roomBookingService = roomBookingService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomBookingDto>>> GetRoomBookings()
    {
        var bookings = await _roomBookingService.GetAllBookingsAsync();
        return Ok(bookings);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoomBookingDto>> GetRoomBooking(int id)
    {
        var booking = await _roomBookingService.GetBookingByIdAsync(id);

        if (booking == null)
        {
            return NotFound("Bokningen hittades inte.");
        }

        return Ok(booking);
    }

    [HttpPost]
    public async Task<ActionResult<RoomBookingDto>> CreateRoomBooking(RoomBookingDto roomBookingDto)
    {
        if (roomBookingDto.EndTime <= roomBookingDto.StartTime)
        {
            return BadRequest("EndTime måste vara efter StartTime.");
        }

        var roomExists = await _context.Rooms.AnyAsync(r => r.RoomId == roomBookingDto.RoomId);

        if (!roomExists)
        {
            return BadRequest("Det angivna rummet finns inte.");
        }

        var bookingDate = roomBookingDto.Date.Date;

        var sameRoomBookings = await _context.RoomBookings
            .Where(rb => rb.RoomId == roomBookingDto.RoomId)
            .ToListAsync();

        bool conflict = sameRoomBookings.Any(rb =>
            rb.Date.Date == bookingDate &&
            roomBookingDto.StartTime < rb.EndTime &&
            roomBookingDto.EndTime > rb.StartTime
        );

        if (conflict)
        {
            return BadRequest("Rummet är redan bokat under den tiden.");
        }

        var roomBooking = new RoomBooking
        {
            RoomId = roomBookingDto.RoomId,
            Date = bookingDate,
            StartTime = roomBookingDto.StartTime,
            EndTime = roomBookingDto.EndTime,
            ActivityId = roomBookingDto.ActivityId,
            BookedByUserId = roomBookingDto.BookedByUserId,
            Status = roomBookingDto.Status
        };

        _context.RoomBookings.Add(roomBooking);
        await _context.SaveChangesAsync();

        var result = new RoomBookingDto
        {
            BookingId = roomBooking.BookingId,
            RoomId = roomBooking.RoomId,
            Date = roomBooking.Date,
            StartTime = roomBooking.StartTime,
            EndTime = roomBooking.EndTime,
            ActivityId = roomBooking.ActivityId,
            BookedByUserId = roomBooking.BookedByUserId,
            Status = roomBooking.Status
        };

        return CreatedAtAction(nameof(GetRoomBooking), new { id = roomBooking.BookingId }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoomBooking(int id, RoomBookingDto roomBookingDto)
    {
        if (id != roomBookingDto.BookingId)
        {
            return BadRequest("ID matchar inte.");
        }

        if (roomBookingDto.EndTime <= roomBookingDto.StartTime)
        {
            return BadRequest("EndTime måste vara efter StartTime.");
        }

        var existingRoomBooking = await _context.RoomBookings.FindAsync(id);

        if (existingRoomBooking == null)
        {
            return NotFound("Bokningen hittades inte.");
        }

        var roomExists = await _context.Rooms.AnyAsync(r => r.RoomId == roomBookingDto.RoomId);

        if (!roomExists)
        {
            return BadRequest("Det angivna rummet finns inte.");
        }

        var bookingDate = roomBookingDto.Date.Date;

        var sameRoomBookings = await _context.RoomBookings
            .Where(rb => rb.RoomId == roomBookingDto.RoomId && rb.BookingId != id)
            .ToListAsync();

        bool conflict = sameRoomBookings.Any(rb =>
            rb.Date.Date == bookingDate &&
            roomBookingDto.StartTime < rb.EndTime &&
            roomBookingDto.EndTime > rb.StartTime
        );

        if (conflict)
        {
            return BadRequest("Rummet är redan bokat under den tiden.");
        }

        existingRoomBooking.RoomId = roomBookingDto.RoomId;
        existingRoomBooking.Date = bookingDate;
        existingRoomBooking.StartTime = roomBookingDto.StartTime;
        existingRoomBooking.EndTime = roomBookingDto.EndTime;
        existingRoomBooking.ActivityId = roomBookingDto.ActivityId;
        existingRoomBooking.BookedByUserId = roomBookingDto.BookedByUserId;
        existingRoomBooking.Status = roomBookingDto.Status;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoomBooking(int id)
    {
        var roomBooking = await _context.RoomBookings.FindAsync(id);

        if (roomBooking == null)
        {
            return NotFound("Bokningen hittades inte.");
        }

        _context.RoomBookings.Remove(roomBooking);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}