using BookingAPI.Data;
using BookingAPI.Models;
using BookingAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly BookingDbContext _context;

    public RoomsController(BookingDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomDto>>> GetRooms()
    {
        var rooms = await _context.Rooms.ToListAsync();

        var result = rooms.Select(r => new RoomDto
        {
            RoomId = r.RoomId,
            RoomName = r.RoomName,
            Capacity = r.Capacity
        }).ToList();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoomDto>> GetRoom(int id)
    {
        var room = await _context.Rooms.FindAsync(id);

        if (room == null)
        {
            return NotFound("Rummet hittades inte.");
        }

        var result = new RoomDto
        {
            RoomId = room.RoomId,
            RoomName = room.RoomName,
            Capacity = room.Capacity
        };

        return Ok(result);
    }

    [HttpGet("{id}/bookings")]
    public async Task<ActionResult<IEnumerable<RoomBooking>>> GetBookingsForRoom(int id)
    {
        var roomExists = await _context.Rooms.AnyAsync(r => r.RoomId == id);

        if (!roomExists)
        {
            return NotFound("Rummet hittades inte.");
        }

        var bookings = await _context.RoomBookings
            .Where(rb => rb.RoomId == id)
            .ToListAsync();

        var sortedBookings = bookings
            .OrderBy(rb => rb.Date)
            .ThenBy(rb => rb.StartTime)
            .ToList();

        return Ok(sortedBookings);
    }

    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<RoomDto>>> GetAvailableRooms(
        [FromQuery] DateTime date,
        [FromQuery] TimeSpan startTime,
        [FromQuery] TimeSpan endTime)
    {
        if (endTime <= startTime)
        {
            return BadRequest("Sluttiden måste vara efter starttiden.");
        }

        var allBookings = await _context.RoomBookings.ToListAsync();

        var conflictingRoomIds = allBookings
            .Where(rb =>
                rb.Date.Date == date.Date &&
                startTime < rb.EndTime &&
                endTime > rb.StartTime)
            .Select(rb => rb.RoomId)
            .Distinct()
            .ToList();

        var allRooms = await _context.Rooms.ToListAsync();

        var availableRooms = allRooms
            .Where(r => !conflictingRoomIds.Contains(r.RoomId))
            .Select(r => new RoomDto
            {
                RoomId = r.RoomId,
                RoomName = r.RoomName,
                Capacity = r.Capacity
            })
            .ToList();

        return Ok(availableRooms);
    }

    [HttpPost]
    public async Task<ActionResult<RoomDto>> CreateRoom(RoomDto roomDto)
    {
        var room = new Room
        {
            RoomName = roomDto.RoomName,
            Capacity = roomDto.Capacity
        };

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();

        var result = new RoomDto
        {
            RoomId = room.RoomId,
            RoomName = room.RoomName,
            Capacity = room.Capacity
        };

        return CreatedAtAction(nameof(GetRoom), new { id = room.RoomId }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoom(int id, RoomDto roomDto)
    {
        if (id != roomDto.RoomId)
        {
            return BadRequest("ID matchar inte.");
        }

        var existingRoom = await _context.Rooms.FindAsync(id);

        if (existingRoom == null)
        {
            return NotFound("Rummet hittades inte.");
        }

        existingRoom.RoomName = roomDto.RoomName;
        existingRoom.Capacity = roomDto.Capacity;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        var room = await _context.Rooms.FindAsync(id);

        if (room == null)
        {
            return NotFound("Rummet hittades inte.");
        }

        var hasBookings = await _context.RoomBookings.AnyAsync(rb => rb.RoomId == id);

        if (hasBookings)
        {
            return BadRequest("Rummet kan inte tas bort eftersom det finns bokningar kopplade till det.");
        }

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}