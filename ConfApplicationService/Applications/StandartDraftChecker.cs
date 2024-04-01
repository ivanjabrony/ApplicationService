using ConfApplicationService.Models.Applications;

namespace ConfApplicationService.Applications;

public static class StandartDraftChecker
{
    public static bool Check(DraftApplication? draftApplication)
    {
        if (draftApplication == null) return false;
        return draftApplication.Description != null ||
               draftApplication.Activity != null ||
               draftApplication.Plan != null ||
               draftApplication.Title != null;
    }
}