using Microsoft.AspNetCore.Mvc;

namespace SOS100GroupProjectMVC.Controllers;

public class MessagesController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}