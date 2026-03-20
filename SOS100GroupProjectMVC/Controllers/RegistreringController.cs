using Microsoft.AspNetCore.Mvc;

namespace SOS100GroupProjectMVC.Controllers;

public class RegistreringController : Controller
{
    // GET
    // Returns the Registrering view (Index.cshtml)
    public IActionResult Index()
    {
        return View();
    }
}