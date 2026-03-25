using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SOS100GroupProjectMVC.Models;

namespace SOS100GroupProjectMVC.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        if (Request.Cookies["userId"] == null)
        {
            return RedirectToAction("Index", "Login");
        }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
