using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Registreringstjansten.Models;

// Data structure that stores the history of status changes for a registration
public class StatusHistorik
{ 
    [Key]
    public int HistoryId { get; set; }
    // Reference to the registration that was changed
    public int RegistrationId { get; set; }
    public string? OldStatus { get; set; }
    public string? NewStatus { get; set; }
    public DateTime ChangedDate { get; set; }
    
    // JsonIgnore prevents circular references during JSON serialization (converting objects to JSON)
    [JsonIgnore]
    public Registrering? Registrering { get; set; }
}