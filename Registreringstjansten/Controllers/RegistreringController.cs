using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Registreringstjansten.Data;
using Registreringstjansten.Models;

// API controller for handling course registrations
[ApiController]
[Route("api/[controller]")]
public class RegistreringController : ControllerBase
{
    // Database context injected via dependency injection
    private readonly AppDbContext _context;

    public RegistreringController(AppDbContext context)
    {
        _context = context;
    }
    
    // Get all registrations
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Registrering>>> GetAll()
    {
        var registrations = await _context.Registreringar.ToListAsync();
        return Ok(registrations);
    }

    // Get one registration by id
    [HttpGet("{id}")]
    public async Task<ActionResult<Registrering>> GetById(int id)
    {
        var registrering = await _context.Registreringar.FindAsync(id);

        if (registrering == null)
        {
            return NotFound();
        }

        return Ok(registrering);
    }
    
    // Get all registrations for a specific user
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<Registrering>>> GetByUser(int userId)
    {
        var registrations = await _context.Registreringar
            .Where(r => r.UserId == userId)
            .ToListAsync();

        return Ok(registrations);
    }
    
    // Get all registrations for a specific course
    [HttpGet("course/{courseId}")]
    public async Task<ActionResult<IEnumerable<Registrering>>> GetByCourse(int courseId)
    {
        var registrations = await _context.Registreringar
            .Where(r => r.CourseId == courseId)
            .ToListAsync();

        return Ok(registrations);
    }
    
    // Create a new registration with default status "väntande"
    [HttpPost]
    public async Task<ActionResult<Registrering>> CreateRegistrering(Registrering registrering)
    {
        registrering.RegistrationDate = DateTime.Now;
        registrering.Status = "väntande";
        _context.Registreringar.Add(registrering);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetByUser), 
            new { userId = registrering.UserId }, registrering);
    }
    
    // Update registration status and save the change to history
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStatus(int id, string newStatus)
    {
        var registrering = await _context.Registreringar.FindAsync(id);
    
        if (registrering == null)
        {
            return NotFound();
        }

        // Create a history entry with old and new status values
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
    
    // Delete a registration permanently from the database
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

    // Get status history for all registrations belonging to a specific user
    [HttpGet("user/{userId}/history")]
    public async Task<ActionResult<IEnumerable<StatusHistorik>>> GetHistoryByUser(int userId)
    {
        // Step 1: Get all registration IDs for this user
        var userRegistrationIds = await _context.Registreringar
            .Where(r => r.UserId == userId)
            .Select(r => r.RegistreringId)
            .ToListAsync();

        // Step 2: Get history entries for those registration IDs
        var history = await _context.StatusHistorik
            .Where(h => userRegistrationIds.Contains(h.RegistrationId))
            .ToListAsync();

        return Ok(history);
    }
    
    // Get status history for all registrations belonging to a specific course
    [HttpGet("course/{courseId}/history")]
    public async Task<ActionResult<IEnumerable<StatusHistorik>>> GetHistoryByCourse(int courseId)
    {
        var courseRegistrationIds = await _context.Registreringar
            .Where(r => r.CourseId == courseId)
            .Select(r => r.RegistreringId)
            .ToListAsync();

        var history = await _context.StatusHistorik
            .Where(h => courseRegistrationIds.Contains(h.RegistrationId))
            .ToListAsync();

        return Ok(history);
    }
}