using Microsoft.AspNetCore.Mvc;
using SOS100GroupProjectMVC.Models;
using System.Text.Json;

namespace SOS100GroupProjectMVC.Controllers;

public class CoursesController : Controller
{
    private readonly HttpClient _httpClient;
    private const string ApiBaseUrl = "https://coursecatalogapi-faededf6g9bbckf6.norwayeast-01.azurewebsites.net/api/courses";

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

    // GET: /Courses/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Courses/Create
    [HttpPost]
    public async Task<IActionResult> Create(CourseViewModel course)
    {
        var json = JsonSerializer.Serialize(course);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(ApiBaseUrl, content);

        if (!response.IsSuccessStatusCode)
            return View(course);

        return RedirectToAction("Index");
    }

    // GET: /Courses/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var response = await _httpClient.GetAsync($"{ApiBaseUrl}/{id}");
        if (!response.IsSuccessStatusCode)
            return NotFound();

        var json = await response.Content.ReadAsStringAsync();
        var course = JsonSerializer.Deserialize<CourseViewModel>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return View(course);
    }

    // POST: /Courses/Edit/5
    [HttpPost]
    public async Task<IActionResult> Edit(int id, CourseViewModel course)
    {
        var json = JsonSerializer.Serialize(course);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"{ApiBaseUrl}/{id}", content);

        if (!response.IsSuccessStatusCode)
            return View(course);

        return RedirectToAction("Index");
    }

    // GET: /Courses/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.GetAsync($"{ApiBaseUrl}/{id}");
        if (!response.IsSuccessStatusCode)
            return NotFound();

        var json = await response.Content.ReadAsStringAsync();
        var course = JsonSerializer.Deserialize<CourseViewModel>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return View(course);
    }

    // POST: /Courses/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var response = await _httpClient.DeleteAsync($"{ApiBaseUrl}/{id}");

        if (!response.IsSuccessStatusCode)
            return NotFound();

        return RedirectToAction("Index");
    }
} 