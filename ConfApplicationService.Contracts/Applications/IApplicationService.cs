using System.Collections.ObjectModel;
using ConfApplicationService.Contracts.Users;
using ConfApplicationService.Models.Applications;
using ConfApplicationService.Models.ResultTypes;

namespace ConfApplicationService.Contracts.Applications;

public interface IApplicationService
{
    
    public IDraftEditor CurrentDraftEditor { get; }
    Task<RepoActResult> CreateNewDraftAsync(ICurrentUserService userService);
    Task<RepoActResult> FinishDraftApplicationAsync(ICurrentUserService userService);
    Task<RepoActResult> GetDraftAsync(ICurrentUserService userService);
    Task<RepoActResult> DeleteDraftAsync(ICurrentUserService userService);
    Task<RepoActResult> SaveDraftChanges(ICurrentUserService userService);
    
    Task<ICollection<FinishedApplication>> GetAllApplicationsAsync();
    Task<ICollection<DraftApplication>> GetAllDraftsAsync();
    Task<RepoActResult> DeleteFinishedApplicationAsync(Guid appId);
    Task<Collection<string>> GetActivitiesTypes();
}