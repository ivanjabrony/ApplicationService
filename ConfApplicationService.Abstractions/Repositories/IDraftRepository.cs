using ConfApplicationService.Contracts.Applications;
using ConfApplicationService.Models.Applications;
using ConfApplicationService.Models.ResultTypes;

namespace ConfApplicationService.Abstractions.Repositories;

public interface IDraftRepository
{
    Task<DraftApplication?> CreateNewDraftForUserAsync(Guid userId);
    Task<DraftApplication?> GetDraftOfUserAsync(Guid userId);
    Task<RepoActResult> DeleteDraftOfUserAsync(Guid userId);
    Task<ICollection<DraftApplication>> GetAllDraftsAsync();
    Task<RepoActResult> SaveDraftChanges(Guid userId);
}