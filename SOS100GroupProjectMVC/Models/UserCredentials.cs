using System.ComponentModel.DataAnnotations;

namespace SOS100GroupProjectMVC.Models;

public class UserCredentials
{
    [Key]
    public string UserName { get; set; }

    public string Salt { get; set; }

    public string Password { get; set; }

    public User User { get; set; }
}