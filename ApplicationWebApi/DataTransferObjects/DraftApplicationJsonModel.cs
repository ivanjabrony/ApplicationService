using ConfApplicationService.Models.Applications;

namespace WebApplication2;

public record DraftApplicationJsonModel(
    Guid AuthorId,
    ActivityType? Activity,
    string? Title,
    string? Plan,
    string? Description = null)
{
    public DraftApplicationJsonModel(DraftApplication application)
        : this(application.AuthorId,
            application.Activity,
            application.Title,
            application.Plan,
            application.Description)
    {}
}