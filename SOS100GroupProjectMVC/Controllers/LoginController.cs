using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SOS100GroupProjectMVC.Data;
using SOS100GroupProjectMVC.DTOs;

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
            return View(model);
        }

        Console.WriteLine("Model state valid");
        var credentials = await _userDbContext.UserCredentials
            .FirstOrDefaultAsync(c => c.UserName == model.UserName);
        
        Console.WriteLine("User found");
        if (credentials == null)
        {
            ModelState.AddModelError("", "Fel användarnamn eller lösenord");
            return View(model);
        }
        Console.WriteLine("User not null");

        //Converts input-string to hash-value with added salt from the found user
        string enteredHash = GetHashFunction(credentials.Salt + model.Password);
        Console.WriteLine("Hash successfully converted");
        if (enteredHash != credentials.Password)
        {
            ModelState.AddModelError("", "Fel användarnamn eller lösenord");
            return View(model);
        }
        Console.WriteLine("Entered hash matches password in database");
        return RedirectToAction("Index", "Home");
    }

    //Logout
    public async Task<IActionResult> Logout()
    {
        return RedirectToAction("Index", "Home");
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