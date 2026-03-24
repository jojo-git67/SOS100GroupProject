namespace KommunicationAPI.DTOs;

public class CreateMessageDto
{
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
}