using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using BookingMVC.Models;

namespace BookingMVC.Controllers;

public class RoomBookingController : Controller
{
    public async Task<IActionResult> Index()
    {
        var model = await LoadPageModelAsync();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking(RoomBookingDto newBooking)
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true
        };

        using var client = new HttpClient(handler);

        var response = await client.PostAsJsonAsync("https://localhost:7285/api/roombookings", newBooking);

        if (response.IsSuccessStatusCode)
        {
            TempData["SuccessMessage"] = "Bokningen skapades.";
            TempData["HighlightLatest"] = "true";
        }
        else
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            TempData["ErrorMessage"] = string.IsNullOrWhiteSpace(errorMessage)
                ? "Något gick fel när bokningen skulle skapas."
                : errorMessage;
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task<RoomBookingPageViewModel> LoadPageModelAsync()
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true
        };

        using var client = new HttpClient(handler);

        var model = new RoomBookingPageViewModel();

        var roomsResponse = await client.GetAsync("https://localhost:7285/api/rooms");
        var bookingsResponse = await client.GetAsync("https://localhost:7285/api/roombookings");

        if (roomsResponse.IsSuccessStatusCode)
        {
            var rooms = await roomsResponse.Content.ReadFromJsonAsync<List<RoomDto>>();
            if (rooms != null)
            {
                model.Rooms = rooms
                    .OrderBy(r => r.RoomName)
                    .ToList();
            }
        }

        if (bookingsResponse.IsSuccessStatusCode)
        {
            var bookings = await bookingsResponse.Content.ReadFromJsonAsync<List<RoomBookingDto>>();

            if (bookings != null)
            {
                var today = DateTime.Today;

                model.Bookings = bookings
                    .OrderBy(b => b.Date.Date < today ? 1 : 0)
                    .ThenBy(b => b.Date.Date < today ? DateTime.MaxValue : b.Date.Date)
                    .ThenBy(b => b.Date.Date < today ? TimeSpan.MaxValue : b.StartTime)
                    .ThenByDescending(b => b.Date.Date < today ? b.Date.Date : DateTime.MinValue)
                    .ThenByDescending(b => b.Date.Date < today ? b.StartTime : TimeSpan.MinValue)
                    .ToList();
            }
        }

        model.NewBooking = new RoomBookingDto
        {
            Date = DateTime.Today,
            StartTime = new TimeSpan(9, 0, 0),
            EndTime = new TimeSpan(10, 0, 0),
            Status = "Created"
        };

        return model;
    }
}