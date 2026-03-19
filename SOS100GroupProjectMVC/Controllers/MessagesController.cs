using Microsoft.AspNetCore.Mvc;
using SOS100GroupProjectMVC.DTOs;

namespace SOS100GroupProjectMVC.Controllers;

public class MessagesController : Controller
{
    private readonly HttpClient _httpClient;

    public MessagesController(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient();
    }
    
    public async Task<IActionResult> Index()
    {
        if (!IsLoggedIn())
        {
            return RedirectToAction("Index", "Login");
        }

        var response = await _httpClient.GetAsync(
            $"http://localhost:5282/api/Kommunication/user/{Request.Cookies["userId"]}");

        if (!response.IsSuccessStatusCode)
        {
            return View(new List<Message>());
        }

        var messages = await response.Content
            .ReadFromJsonAsync<List<Message>>();
        

        return View(messages);
    }

    public async Task<IActionResult> CreateMessage()
    {
        if (!IsLoggedIn())
        {
            return RedirectToAction("Index", "Login");
        }
        
        var response = await _httpClient.GetAsync(
            $"http://localhost:5041/api/Registrering/user/{Request.Cookies["userId"]}");

        if (!response.IsSuccessStatusCode)
        {
            return View(new List<Registration>());
        }
        
        var registrations = await response.Content
            .ReadFromJsonAsync<List<Registration>>();
        
        return View(registrations);
    }

    public bool IsLoggedIn()
    {
        var userId = Request.Cookies["userId"];

        if (string.IsNullOrEmpty(userId))
        {
            return false;
        }
        return true;
    }

}