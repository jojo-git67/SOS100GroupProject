using Microsoft.AspNetCore.Mvc;
using SOS100GroupProjectMVC.Data;
using SOS100GroupProjectMVC.DTOs;
using SOS100GroupProjectMVC.Models;

namespace SOS100GroupProjectMVC.Controllers;

public class MessagesController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly UserDbContext _context;

    public MessagesController(IHttpClientFactory factory, UserDbContext context)
    {
        _httpClient = factory.CreateClient();
        _context = context;
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

    public IActionResult CreateMessage()
    {
        if (!IsLoggedIn())
        {
            return RedirectToAction("Index", "Login");
        }

        var users = _context.Users.ToList();
        ViewBag.Users = users;

        return View();
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

    [HttpPost]
    public async Task<IActionResult> SendMessage(CreateMessageDto dto)
    {
        if (!IsLoggedIn())
        {
            return RedirectToAction("Index", "Login");
        }

        // Set sender from cookie
        dto.SenderId = int.Parse(Request.Cookies["userId"]);

        var response = await _httpClient.PostAsJsonAsync(
            "http://localhost:5282/api/Kommunication", dto);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError("", "Kunde inte skicka meddelandet");
            return View("CreateMessage", dto);
        }

        return RedirectToAction("Index", "Messages");
    }
    
    public List<User> GetUsers()
    {
        return _context.Users.ToList();
    }

}