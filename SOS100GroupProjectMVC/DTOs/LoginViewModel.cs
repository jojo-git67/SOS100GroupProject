using System.ComponentModel.DataAnnotations;

namespace SOS100GroupProjectMVC.DTOs;

public class LoginViewModel
{
    [Required(ErrorMessage = "Användarnamn är obligatoriskt")]
    public string UserName { get; set; }
    
    [Required(ErrorMessage = "Lösenord är obligatoriskt")]
    public string Password { get; set; }
}