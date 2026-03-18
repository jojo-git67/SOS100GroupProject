using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using BookingMVC.Models;

namespace BookingMVC.Controllers;

public class RoomBookingController : Controller
{
    public async Task<IActionResult> Index()
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
                model.Bookings = bookings;
            }
        }

        return View(model);
    }
}