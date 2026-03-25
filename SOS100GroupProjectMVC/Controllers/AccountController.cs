using Microsoft.AspNetCore.Mvc;

namespace SOS100GroupProjectMVC.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(int userId, string role)
        {
            Response.Cookies.Append("UserId", userId.ToString());
            Response.Cookies.Append("Role", role);

            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("UserId");
            Response.Cookies.Delete("Role");

            return RedirectToAction("Login");
        }
    }
}