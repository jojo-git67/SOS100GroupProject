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
}