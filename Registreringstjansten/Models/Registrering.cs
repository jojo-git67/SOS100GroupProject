using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Registreringstjansten.Models;

// Data structure for a course registration containing user, course, date and status information
public class Registrering
{
    [Key]
    public int RegistreringId { get; set; }
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public DateTime RegistrationDate { get; set; }
    // Status values: väntande, godkänd or nekad
    public string? Status { get; set; }
    
    // JsonIgnore prevents circular references during JSON serialization (converting objects to JSON)
    [JsonIgnore]
    public ICollection<StatusHistorik>? StatusHistorik { get; set; }
}