namespace KommunicationAPI.Models;

public class Message
{
    
    public int Id { get; set; }

    public int SenderId { get; set; }

    public int ReceiverId { get; set; }
    
    public string Title { get; set; }

    public string Content { get; set; }

    public DateTime timestamp { get; set; }

    public bool IsRead { get; set; }
}