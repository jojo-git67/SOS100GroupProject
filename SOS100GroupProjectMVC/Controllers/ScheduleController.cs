using Microsoft.AspNetCore.Mvc;
using SOS100GroupProjectMVC.Models;
using System.Net.Http.Json;

namespace SOS100GroupProjectMVC.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly HttpClient _httpClient;

        public ScheduleController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5160");
        }

        public async Task<IActionResult> Index()
        {
            var activities = new List<ScheduleActivity>();

            try
            {
                var result = await _httpClient.GetFromJsonAsync<List<ScheduleActivity>>("/api/ScheduleActivities");
                if (result != null)
                {
                    activities = result;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"];
            }

            if (TempData["Success"] != null)
            {
                ViewBag.Success = TempData["Success"];
            }

            return View(activities);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int CourseId, int UserId, string Title, string Date, string StartTime, string EndTime, int RoomId)
        {
            try
            {
                var activity = new ScheduleActivity
                {
                    CourseId = CourseId,
                    UserId = UserId,
                    Title = Title,
                    Date = DateOnly.Parse(Date),
                    StartTime = TimeSpan.Parse(StartTime),
                    EndTime = TimeSpan.Parse(EndTime),
                    RoomId = RoomId
                };

                var response = await _httpClient.PostAsJsonAsync("/api/ScheduleActivities", activity);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Aktivitet skapades!";
                    return RedirectToAction(nameof(Index));
                }

                var felmeddelande = await response.Content.ReadAsStringAsync();
                TempData["Error"] = $"API-fel {(int)response.StatusCode}: {felmeddelande}";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var selectedActivity = await _httpClient.GetFromJsonAsync<ScheduleActivity>($"/api/ScheduleActivities/{id}");

                if (selectedActivity == null)
                {
                    return NotFound();
                }

                return View(selectedActivity);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ScheduleActivity activity)
        {
            if (!ModelState.IsValid)
            {
                return View(activity);
            }

            try
            {
                var response = await _httpClient.PutAsJsonAsync($"/api/ScheduleActivities/{id}", activity);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Error = "Kunde inte uppdatera aktivitet.";
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View(activity);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var selectedActivity = await _httpClient.GetFromJsonAsync<ScheduleActivity>($"/api/ScheduleActivities/{id}");

                if (selectedActivity == null)
                {
                    return NotFound();
                }

                return View(selectedActivity);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _httpClient.DeleteAsync($"/api/ScheduleActivities/{id}");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}