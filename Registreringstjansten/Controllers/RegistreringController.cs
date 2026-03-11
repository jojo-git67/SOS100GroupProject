using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Registreringstjansten.Data;
using Registreringstjansten.Models;

[ApiController]
[Route("api/[controller]")]
public class RegistreringController : ControllerBase
{
    private readonly AppDbContext _context;

    public RegistreringController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<Registrering>>> GetByUser(int userId)
    {
        var registrations = await _context.Registreringar
            .Where(r => r.UserId == userId)
            .ToListAsync();

        return Ok(registrations);
    }
    
    [HttpGet("course/{courseId}")]
    public async Task<ActionResult<IEnumerable<Registrering>>> GetByCourse(int courseId)
    {
        var registrations = await _context.Registreringar
            .Where(r => r.CourseId == courseId)
            .ToListAsync();

        return Ok(registrations);
    }
    
    [HttpPost]
    public async Task<ActionResult<Registrering>> CreateRegistrering(Registrering registrering)
    {
        registrering.RegistrationDate = DateTime.Now;
        registrering.Status = "pending";
        _context.Registreringar.Add(registrering);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetByUser), 
            new { userId = registrering.UserId }, registrering);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStatus(int id, string newStatus)
    {
        var registrering = await _context.Registreringar.FindAsync(id);
    
        if (registrering == null)
        {
            return NotFound();
        }

        var history = new StatusHistorik
        {
            RegistrationId = id,
            OldStatus = registrering.Status,
            NewStatus = newStatus,
            ChangedDate = DateTime.Now
        };

        registrering.Status = newStatus;
        _context.StatusHistorik.Add(history);
        await _context.SaveChangesAsync();

        return Ok(registrering);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRegistrering(int id)
    {
        var registrering = await _context.Registreringar.FindAsync(id);
    
        if (registrering == null)
        {
            return NotFound();
        }

        _context.Registreringar.Remove(registrering);
        await _context.SaveChangesAsync();

        return NoContent();
    }

// Real history endpoint
    [HttpGet("user/{userId}/history")]
    public async Task<ActionResult<IEnumerable<StatusHistorik>>> GetHistoryByUser(int userId)
    {
        // First get all registrationIds for this user
        var userRegistrationIds = await _context.Registreringar
            .Where(r => r.UserId == userId)
            .Select(r => r.RegistreringId)
            .ToListAsync();

        // Then get history for those registrationIds
        var history = await _context.StatusHistorik
            .Where(h => userRegistrationIds.Contains(h.RegistrationId))
            .ToListAsync();

        return Ok(history);
    }
}