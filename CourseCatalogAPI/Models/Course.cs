namespace CourseCatalogAPI.Models;

public class Course
{
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int TeacherId { get; set; }
    public int MaxParticipants { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}