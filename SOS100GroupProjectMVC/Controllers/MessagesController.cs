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
        var userId = Request.Cookies["userId"];

        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Index", "Login");
        }

        var response = await _httpClient.GetAsync(
            $"http://localhost:5282/api/Kommunication/user/{userId}");

        if (!response.IsSuccessStatusCode)
        {
            return View(new List<Message>());
        }

        var messages = await response.Content
            .ReadFromJsonAsync<List<Message>>();

        return View(messages);
    }
}