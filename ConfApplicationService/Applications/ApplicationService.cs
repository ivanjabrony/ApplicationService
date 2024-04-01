using System.Collections.ObjectModel;
using ConfApplicationService.Abstractions.Repositories;
using ConfApplicationService.Contracts.Applications;
using ConfApplicationService.Contracts.Users;
using ConfApplicationService.Models.Applications;
using ConfApplicationService.Models.ResultTypes;

namespace ConfApplicationService.Applications;

public class ApplicationService(
        IApplicationRepository applicationRepository,
        IDraftRepository draftRepository,
        IDraftEditor draftEditor)
    : IApplicationService
{
    public IDraftEditor CurrentDraftEditor => draftEditor;
    public async Task<RepoActResult> CreateNewDraftAsync(ICurrentUserService userService)
    {
        if (userService?.User == null) return new RepoActResult.FailRepoAct("User service undefined");

        if (await draftRepository.GetDraftOfUserAsync(userService.User.UserId) != null)
            return new RepoActResult.FailRepoAct("Draft already existed");
        
        var draft = await draftRepository.CreateNewDraftForUserAsync(userService.User.UserId);
        if (draft == null) return new RepoActResult.FailRepoAct("Couldn't create new draft");
        draftEditor.EditingDraft = draft;
        
        return new RepoActResult.SuccessRepoAct();
    }

    public async Task<RepoActResult> FinishDraftApplicationAsync(ICurrentUserService userService)
    {
        if (userService?.User == null) return new RepoActResult.FailRepoAct("User service undefined");
        if (draftEditor.EditingDraft == null) return new RepoActResult.FailRepoAct("User service undefined");
        if (!draftEditor.CheckDraft()) return new RepoActResult.FailRepoAct("Draft is not correct");
        
        var draft = draftEditor.EditingDraft;
        var finishedApplication = await applicationRepository
            .CreateFinishedApplicationAsync(
                userService.User.UserId,
                draft.Activity ?? throw new ArgumentNullException(),
                draft.Title ?? throw new ArgumentNullException(),
                draft.Plan ?? throw new ArgumentNullException(),
                draft.Description);

        if (finishedApplication == null) 
            return new RepoActResult.FailRepoAct("Unable to create application");
        
        return await draftRepository.DeleteDraftOfUserAsync(userService.User.UserId);
    }

    public async Task<RepoActResult> GetDraftAsync(ICurrentUserService userService)
    {
        if (userService?.User == null) return new RepoActResult.FailRepoAct("User service undefined");

        var draft = await draftRepository.GetDraftOfUserAsync(userService.User.UserId);
        if (draft == null) 
            return new RepoActResult.FailRepoAct("Couldn't get draft of user");
        
        draftEditor.EditingDraft = draft;
        return new RepoActResult.SuccessRepoAct();
    }

    public async Task<RepoActResult> DeleteDraftAsync(ICurrentUserService userService)
    {
        if (userService?.User == null) return new RepoActResult.FailRepoAct("User service undefined");
        if (draftEditor.EditingDraft == null) return new RepoActResult.FailRepoAct("No active draft");

        return await draftRepository.DeleteDraftOfUserAsync(userService.User.UserId);
    }

    public async Task<RepoActResult> SaveDraftChanges(ICurrentUserService userService)
    {
        if (userService?.User == null) return new RepoActResult.FailRepoAct("User service undefined");
        if (draftEditor.EditingDraft == null) return new RepoActResult.FailRepoAct("No active draft");

        var draft = await draftRepository.GetDraftOfUserAsync(userService.User.UserId);
        if (draft == null) return new RepoActResult.FailRepoAct("Couldn't get draft of user");
        
        draft.Title = draftEditor.EditingDraft.Plan;
        draft.Activity = draftEditor.EditingDraft.Activity;
        draft.Plan = draftEditor.EditingDraft.Plan;
        draft.Description = draftEditor.EditingDraft.Description;
        
        return await draftRepository.SaveDraftChanges(userService.User.UserId);
    }

    public async Task<ICollection<FinishedApplication>> GetAllApplicationsAsync()
    {
        return await applicationRepository.GetAllFinishedApplicationsAsync();
    }

    public async Task<ICollection<DraftApplication>> GetAllDraftsAsync()
    {
        return await draftRepository.GetAllDraftsAsync();
    }

    public async Task<RepoActResult> DeleteFinishedApplicationAsync(Guid appId)
    {
        return await applicationRepository.DeleteFinishedApplication(appId);
    }

    public async Task<Collection<string>> GetActivitiesTypes()
    {
        var types = new Collection<string>
        {
            "Lecture",
            "Discussion",
            "MasterClass"
        };
        return types;
    }
}