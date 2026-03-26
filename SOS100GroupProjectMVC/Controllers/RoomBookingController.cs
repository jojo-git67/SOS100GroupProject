using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using SOS100GroupProjectMVC.Models;

namespace SOS100GroupProjectMVC.Controllers;

public class RoomBookingController : Controller
{
    public async Task<IActionResult> Index()
    {
        if (!TryGetLoggedInUserId(out _))
        {
            return RedirectToAction("Index", "Login");
        }

        var model = await LoadPageModelAsync();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        if (!TryGetLoggedInUserId(out _))
        {
            return RedirectToAction("Index", "Login");
        }

        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true
        };

        using var client = new HttpClient(handler);

        var bookingResponse = await client.GetAsync($"https://localhost:7285/api/roombookings/{id}");
        var roomsResponse = await client.GetAsync("https://localhost:7285/api/rooms");

        if (!bookingResponse.IsSuccessStatusCode)
        {
            TempData["ErrorMessage"] = "Bokningen kunde inte hämtas.";
            return RedirectToAction(nameof(Index));
        }

        var booking = await bookingResponse.Content.ReadFromJsonAsync<RoomBookingDto>();
        var rooms = new List<RoomDto>();

        if (roomsResponse.IsSuccessStatusCode)
        {
            var roomList = await roomsResponse.Content.ReadFromJsonAsync<List<RoomDto>>();
            if (roomList != null)
            {
                rooms = roomList;
            }
        }

        if (booking == null)
        {
            TempData["ErrorMessage"] = "Bokningen kunde inte hämtas.";
            return RedirectToAction(nameof(Index));
        }

        var model = new RoomBookingPageViewModel
        {
            Rooms = rooms,
            NewBooking = booking
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking(RoomBookingDto newBooking)
    {
        if (!TryGetLoggedInUserId(out var userId))
        {
            return RedirectToAction("Index", "Login");
        }

        newBooking.BookedByUserId = userId;

        if (string.IsNullOrWhiteSpace(newBooking.Status))
        {
            newBooking.Status = "Created";
        }

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

    [HttpPost]
    public async Task<IActionResult> UpdateBooking(RoomBookingDto booking)
    {
        if (!TryGetLoggedInUserId(out var userId))
        {
            return RedirectToAction("Index", "Login");
        }

        booking.BookedByUserId = userId;

        if (string.IsNullOrWhiteSpace(booking.Status))
        {
            booking.Status = "Created";
        }

        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true
        };

        using var client = new HttpClient(handler);

        var response = await client.PutAsJsonAsync(
            $"https://localhost:7285/api/roombookings/{booking.BookingId}",
            booking);

        if (response.IsSuccessStatusCode)
        {
            TempData["SuccessMessage"] = "Bokningen uppdaterades.";
            return RedirectToAction(nameof(Index));
        }

        var errorMessage = await response.Content.ReadAsStringAsync();

        var roomsResponse = await client.GetAsync("https://localhost:7285/api/rooms");
        var rooms = new List<RoomDto>();

        if (roomsResponse.IsSuccessStatusCode)
        {
            var roomList = await roomsResponse.Content.ReadFromJsonAsync<List<RoomDto>>();
            if (roomList != null)
            {
                rooms = roomList;
            }
        }

        TempData["ErrorMessage"] = string.IsNullOrWhiteSpace(errorMessage)
            ? "Något gick fel när bokningen skulle uppdateras."
            : errorMessage;

        var model = new RoomBookingPageViewModel
        {
            Rooms = rooms,
            NewBooking = booking
        };

        return View("Edit", model);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteBooking(int id)
    {
        if (!TryGetLoggedInUserId(out _))
        {
            return RedirectToAction("Index", "Login");
        }

        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true
        };

        using var client = new HttpClient(handler);

        var response = await client.DeleteAsync($"https://localhost:7285/api/roombookings/{id}");

        if (response.IsSuccessStatusCode)
        {
            TempData["SuccessMessage"] = "Bokningen avbokades.";
        }
        else
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            TempData["ErrorMessage"] = string.IsNullOrWhiteSpace(errorMessage)
                ? "Något gick fel när bokningen skulle tas bort."
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
                model.Rooms = rooms;
            }
        }

        if (bookingsResponse.IsSuccessStatusCode)
        {
            var bookings = await bookingsResponse.Content.ReadFromJsonAsync<List<RoomBookingDto>>();
            if (bookings != null)
            {
                if (TryGetLoggedInUserId(out var userId))
                {
                    bookings = bookings
                        .Where(b => b.BookedByUserId == userId)
                        .ToList();
                }

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

    private bool TryGetLoggedInUserId(out int userId)
    {
        userId = 0;

        var userIdCookie = Request.Cookies["userId"];

        if (string.IsNullOrWhiteSpace(userIdCookie))
        {
            return false;
        }

        return int.TryParse(userIdCookie, out userId);
    }
}