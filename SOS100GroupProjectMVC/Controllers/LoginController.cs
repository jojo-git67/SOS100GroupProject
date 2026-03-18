using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SOS100GroupProjectMVC.Data;
using SOS100GroupProjectMVC.DTOs;
using SOS100GroupProjectMVC.Models;

namespace SOS100GroupProjectMVC.Controllers;

public class LoginController : Controller
{
    private readonly UserDbContext _userDbContext;

    public LoginController(UserDbContext userDbContext)
    {
        _userDbContext = userDbContext;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Index");
        }

        Console.WriteLine("Model state valid");

        var credentials = await _userDbContext.UserCredentials
            .FirstOrDefaultAsync(c => c.UserName == model.UserName);

        if (credentials == null)
        {
            ModelState.AddModelError("", "Fel användarnamn eller lösenord");
            return View("Index");
        }

        Console.WriteLine("User found");

        // Converts input-string to hash-value with added salt from the found user
        string enteredHash = GetHashFunction(credentials.Salt + model.Password);
        Console.WriteLine("Hash successfully converted");

        // Compare entered hash with saved password hash
        if (model.Password != credentials.Password)
        {
            ModelState.AddModelError("", "Fel användarnamn eller lösenord");
            return View("Index");
        }

        Console.WriteLine("Entered hash matches password in database");

        // Get full user object to access UserId and Role
        var user = await _userDbContext.Users
            .FirstOrDefaultAsync(u => u.UserName == model.UserName);

        if (user == null)
        {
            ModelState.AddModelError("", "Användaren hittades inte");
            return View("Index");
        }

        // Set cookies for other services
        Response.Cookies.Append("userId", user.UserId.ToString());
        Response.Cookies.Append("role", user.Role);

        return RedirectToAction("Index", "Home");
    }

    // Logout
    public IActionResult Logout()
    {
        Response.Cookies.Delete("userId");
        Response.Cookies.Delete("role");

        return RedirectToAction("Index", "Login");
    }

    private string GetHashFunction(string input)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }
}