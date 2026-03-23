using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using SOS100GroupProjectMVC.Models;

namespace SOS100GroupProjectMVC.Controllers;

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
                model.Rooms = rooms;
        }

        if (bookingsResponse.IsSuccessStatusCode)
        {
            var bookings = await bookingsResponse.Content.ReadFromJsonAsync<List<RoomBookingDto>>();
            if (bookings != null)
            {
                model.Bookings = bookings
                    .OrderByDescending(b => b.Date)
                    .ThenByDescending(b => b.StartTime)
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