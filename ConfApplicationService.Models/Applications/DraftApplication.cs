namespace ConfApplicationService.Models.Applications;

public record DraftApplication
{
    public ActivityType? Activity { get; set; } = null;
    public string? Title { get; set; } = string.Empty;
    public string? Plan { get; set; } = null;
    public string? Description { get; set; } = null;
    
    public DateTime CreatingDate = DateTime.Now;
    public Guid AuthorId { get; set; }
    public Guid DraftApplicationId { get; set; } = new();
}