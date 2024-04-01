using ConfApplicationService.Models.Applications;
using ConfApplicationService.Models.ResultTypes;

namespace ConfApplicationService.Abstractions.Repositories;

public interface IApplicationRepository
{
    Task<FinishedApplication> CreateFinishedApplicationAsync(Guid authorId, ActivityType activityType, string title, string plan, string? description = null, Guid applicationId = new());
    Task<ICollection<FinishedApplication>> GetAllApplicationsOfUserAsync(Guid userId);
    Task<FinishedApplication?> GetFinishedApplicationAsync(Guid appId);

    Task<RepoActResult> DeleteFinishedApplication(Guid appId);
    Task<ICollection<FinishedApplication>> GetAllFinishedApplicationsAsync();
}