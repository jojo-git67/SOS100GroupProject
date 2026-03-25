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

            return View(activities);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ScheduleActivity activity)
        {
            if (!ModelState.IsValid)
            {
                return View(activity);
            }

            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/ScheduleActivities", activity);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Error = "Kunde inte skapa aktivitet.";
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View(activity);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var activities = await _httpClient.GetFromJsonAsync<List<ScheduleActivity>>("/api/ScheduleActivities");
                var selectedActivity = activities?.FirstOrDefault(a => a.ActivityId == id);

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
                var activities = await _httpClient.GetFromJsonAsync<List<ScheduleActivity>>("/api/ScheduleActivities");
                var selectedActivity = activities?.FirstOrDefault(a => a.ActivityId == id);

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
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}