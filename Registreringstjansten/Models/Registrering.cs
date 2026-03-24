using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Registreringstjansten.Models;

public class Registrering
{
    [Key]
    public int RegistreringId { get; set; }
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string? Status { get; set; }
    
    [JsonIgnore]
    public ICollection<StatusHistorik>? StatusHistorik { get; set; }
}