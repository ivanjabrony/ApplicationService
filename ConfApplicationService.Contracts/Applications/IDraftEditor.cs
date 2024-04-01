using ConfApplicationService.Models.Applications;

namespace ConfApplicationService.Contracts.Applications;

public interface IDraftEditor
{
    public DraftApplication? EditingDraft { get; set; }
    void ChangeActivity(ActivityType? newActivityType);
    void ChangeTitle(string? newTitle);
    void ChangePlan(string? newPlan);
    void ChangeDescription(string? newDescription);
    bool CheckDraft();
}