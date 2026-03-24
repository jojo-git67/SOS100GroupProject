using System.ComponentModel.DataAnnotations;

namespace SOS100GroupProjectMVC.DTOs;

public class Registration
{
    [Key]
    public int RegistreringId { get; set; }
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string? Status { get; set; }
}