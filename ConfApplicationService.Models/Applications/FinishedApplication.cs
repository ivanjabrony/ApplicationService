namespace ConfApplicationService.Models.Applications;

public record FinishedApplication(
    Guid AuthorId,
    ActivityType Activity,
    string Title,
    string Plan,
    DateTime CreationTime,
    string? Description = null,
    Guid FinishedApplicationId = new());