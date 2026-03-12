using BookingAPI.Data;
using BookingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomBookingsController : ControllerBase
{
    private readonly BookingDbContext _context;

    public RoomBookingsController(BookingDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomBooking>>> GetRoomBookings()
    {
        return await _context.RoomBookings.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoomBooking>> GetRoomBooking(int id)
    {
        var roomBooking = await _context.RoomBookings.FindAsync(id);

        if (roomBooking == null)
        {
            return NotFound();
        }

        return roomBooking;
    }

    [HttpPost]
    public async Task<ActionResult<RoomBooking>> CreateRoomBooking(RoomBooking roomBooking)
    {
        _context.RoomBookings.Add(roomBooking);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRoomBooking), new { id = roomBooking.BookingId }, roomBooking);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoomBooking(int id, RoomBooking roomBooking)
    {
        if (id != roomBooking.BookingId)
        {
            return BadRequest();
        }

        var existingRoomBooking = await _context.RoomBookings.FindAsync(id);

        if (existingRoomBooking == null)
        {
            return NotFound();
        }

        existingRoomBooking.RoomId = roomBooking.RoomId;
        existingRoomBooking.Date = roomBooking.Date;
        existingRoomBooking.StartTime = roomBooking.StartTime;
        existingRoomBooking.EndTime = roomBooking.EndTime;
        existingRoomBooking.ActivityId = roomBooking.ActivityId;
        existingRoomBooking.BookedByUserId = roomBooking.BookedByUserId;
        existingRoomBooking.Status = roomBooking.Status;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoomBooking(int id)
    {
        var roomBooking = await _context.RoomBookings.FindAsync(id);

        if (roomBooking == null)
        {
            return NotFound();
        }

        _context.RoomBookings.Remove(roomBooking);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}