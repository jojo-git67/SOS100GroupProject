using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Registreringstjansten.Models;

public class StatusHistorik
{ 
    [Key]
    public int HistoryId { get; set; }
    public int RegistrationId { get; set; }
    public string? OldStatus { get; set; }
    public string? NewStatus { get; set; }
    public DateTime ChangedDate { get; set; }
    
    [JsonIgnore]
    public Registrering? Registrering { get; set; }
}