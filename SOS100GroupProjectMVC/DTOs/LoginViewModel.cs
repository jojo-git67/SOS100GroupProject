using System.ComponentModel.DataAnnotations;

namespace SOS100GroupProjectMVC.DTOs;

public class LoginViewModel
{
    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }
}