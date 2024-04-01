using ConfApplicationService.Contracts.Applications;
using ConfApplicationService.Models.Applications;

namespace ConfApplicationService.Applications;

public class CurrentDraftEditor : IDraftEditor
{
    public DraftApplication? EditingDraft { get; set; }

    public void ChangeActivity(ActivityType? newActivityType)
    {
        if (EditingDraft == null) throw new NullReferenceException(nameof(EditingDraft));
        EditingDraft.Activity = newActivityType;
    }

    public void ChangeTitle(string? newTitle)
    {
        if (EditingDraft == null) throw new NullReferenceException(nameof(EditingDraft));
        EditingDraft.Title = newTitle;
    }

    public void ChangePlan(string? newPlan)
    {
        if (EditingDraft == null) throw new NullReferenceException(nameof(EditingDraft));
        EditingDraft.Plan = newPlan;
    }

    public void ChangeDescription(string? newDescription)
    {
        if (EditingDraft == null) throw new NullReferenceException(nameof(EditingDraft));
        EditingDraft.Description = newDescription;
    }

    public bool CheckDraft()
    {
        return StandartDraftChecker.Check(EditingDraft);
    }
}