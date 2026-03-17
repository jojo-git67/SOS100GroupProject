using Microsoft.AspNetCore.Mvc;
using SOS100GroupProjectMVC.Models;
using System.Text.Json;

namespace SOS100GroupProjectMVC.Controllers;

public class CoursesController : Controller
{
    private readonly HttpClient _httpClient;
    private const string ApiBaseUrl = "http://localhost:5149/api/courses";
    

    public CoursesController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    // GET: /Courses
    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync(ApiBaseUrl);
        if (!response.IsSuccessStatusCode)
            return View(new List<CourseViewModel>());

        var json = await response.Content.ReadAsStringAsync();
        var courses = JsonSerializer.Deserialize<List<CourseViewModel>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return View(courses);
    }

    // GET: /Courses/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var response = await _httpClient.GetAsync($"{ApiBaseUrl}/{id}");
        if (!response.IsSuccessStatusCode)
            return NotFound();

        var json = await response.Content.ReadAsStringAsync();
        var course = JsonSerializer.Deserialize<CourseViewModel>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return View(course);
    }
}