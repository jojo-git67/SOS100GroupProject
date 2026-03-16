using Microsoft.AspNetCore.Mvc;
using ScheduleService.Models;

namespace ScheduleService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleActivitiesController : ControllerBase
    {
        private static readonly List<ScheduleActivity> Activities = new();

        private bool HasAccessToManageSchedule()
        {
            if (!Request.Headers.TryGetValue("Role", out var role))
            {
                return false;
            }

            return role == "CourseAdmin" || role == "IT-admin";
        }

        [HttpGet]
        public ActionResult<IEnumerable<ScheduleActivity>> GetAll()
        {
            return Ok(Activities);
        }

        [HttpGet("{id}")]
        public ActionResult<ScheduleActivity> GetById(int id)
        {
            var activity = Activities.FirstOrDefault(a => a.ActivityId == id);

            if (activity == null)
            {
                return NotFound();
            }

            return Ok(activity);
        }

        [HttpPost]
        public ActionResult<ScheduleActivity> Create(ScheduleActivity activity)
        {
            if (!HasAccessToManageSchedule())
            {
                return StatusCode(403, "Du har inte behörighet.");
            }

            activity.ActivityId = Activities.Count > 0 ? Activities.Max(a => a.ActivityId) + 1 : 1;
            Activities.Add(activity);

            return CreatedAtAction(nameof(GetById), new { id = activity.ActivityId }, activity);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, ScheduleActivity updatedActivity)
        {
            if (!HasAccessToManageSchedule())
            {
                return StatusCode(403, "Du har inte behörighet.");
            }

            var existingActivity = Activities.FirstOrDefault(a => a.ActivityId == id);

            if (existingActivity == null)
            {
                return NotFound();
            }

            existingActivity.CourseId = updatedActivity.CourseId;
            existingActivity.Title = updatedActivity.Title;
            existingActivity.Date = updatedActivity.Date;
            existingActivity.StartTime = updatedActivity.StartTime;
            existingActivity.EndTime = updatedActivity.EndTime;
            existingActivity.RoomName = updatedActivity.RoomName;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!HasAccessToManageSchedule())
            {
                return StatusCode(403, "Du har inte behörighet.");
            }

            var activity = Activities.FirstOrDefault(a => a.ActivityId == id);

            if (activity == null)
            {
                return NotFound();
            }

            Activities.Remove(activity);
            return NoContent();
        }
    }
}
