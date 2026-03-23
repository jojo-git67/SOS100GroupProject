namespace SOS100GroupProjectMVC.DTOs;

public class CreateMessageDto
{
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public string Content { get; set; }
    public string Title { get; set; }
}