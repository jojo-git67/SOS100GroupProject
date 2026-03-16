using System.ComponentModel.DataAnnotations;

namespace SOS100GroupProjectMVC.Models;

public class User
{
    [Key]
    public int UserId { get; set; }

    public string UserName { get; set; }

    public string UserEmail { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public UserCredentials Credentials { get; set; }
    
}
