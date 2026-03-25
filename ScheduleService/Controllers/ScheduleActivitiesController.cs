using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScheduleService.Data;
using ScheduleService.Models;

namespace ScheduleService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleActivitiesController : ControllerBase
    {
        private readonly ScheduleDbContext _context;

        public ScheduleActivitiesController(ScheduleDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduleActivity>>> GetAll()
        {
            return Ok(await _context.ScheduleActivities.ToListAsync());
        }

        [HttpGet("course/{courseId}")]
        public async Task<ActionResult<IEnumerable<ScheduleActivity>>> GetByCourse(int courseId)
        {
            var activities = await _context.ScheduleActivities
                .Where(a => a.CourseId == courseId)
                .ToListAsync();

            return Ok(activities);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<ScheduleActivity>>> GetByUser(int userId)
        {
            var activities = await _context.ScheduleActivities
                .Where(a => a.UserId == userId)
                .ToListAsync();

            return Ok(activities);
        }

        [HttpPost]
        public async Task<ActionResult<ScheduleActivity>> Create(ScheduleActivity activity)
        {
            _context.ScheduleActivities.Add(activity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = activity.ActivityId }, activity);
        }

        [HttpPut("{activityId}")]
        public async Task<IActionResult> Update(int activityId, ScheduleActivity updatedActivity)
        {
            var existingActivity = await _context.ScheduleActivities
                .FirstOrDefaultAsync(a => a.ActivityId == activityId);

            if (existingActivity == null)
            {
                return NotFound();
            }

            existingActivity.CourseId = updatedActivity.CourseId;
            existingActivity.UserId = updatedActivity.UserId;
            existingActivity.Title = updatedActivity.Title;
            existingActivity.Date = updatedActivity.Date;
            existingActivity.StartTime = updatedActivity.StartTime;
            existingActivity.EndTime = updatedActivity.EndTime;
            existingActivity.RoomId = updatedActivity.RoomId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{activityId}")]
        public async Task<IActionResult> Delete(int activityId)
        {
            var activity = await _context.ScheduleActivities
                .FirstOrDefaultAsync(a => a.ActivityId == activityId);

            if (activity == null)
            {
                return NotFound();
            }

            _context.ScheduleActivities.Remove(activity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}