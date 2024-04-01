using ConfApplicationService.Abstractions.Repositories;
using ConfApplicationService.Implementation.DataContext;
using ConfApplicationService.Models.Applications;
using ConfApplicationService.Models.ResultTypes;
using Microsoft.EntityFrameworkCore;

namespace ConfApplicationService.Implementation.Repositories;

public class ApplicationRepository(ApplicationServiceContext applicationContext) : IApplicationRepository
{
    public async Task<FinishedApplication> CreateFinishedApplicationAsync(
        Guid authorId,
        ActivityType activityType,
        string title,
        string plan,
        string? description = null,
        Guid applicationId = new())
    {
        var application = new FinishedApplication(
            authorId,
            activityType,
            title, plan, DateTime.Now ,description, applicationId);
        await applicationContext.FinishedApplications.AddAsync(application);
        await applicationContext.SaveChangesAsync();
        return application;
    }

    public async Task<ICollection<FinishedApplication>> GetAllApplicationsOfUserAsync(Guid userId)
    {
        return await applicationContext.FinishedApplications
            .Where(x => x.AuthorId == userId)
            .ToListAsync();
    }

    public async Task<FinishedApplication?> GetFinishedApplicationAsync(Guid appId)
    {
        return await applicationContext.FinishedApplications.FindAsync(appId);
    }

    public async Task<RepoActResult> DeleteFinishedApplication(Guid appId)
    {
        var application = await applicationContext.FinishedApplications.FindAsync(appId);
        if (application == null) return new RepoActResult.FailRepoAct("Couldn't find such an application");

        applicationContext.FinishedApplications.Remove(application);
        return new RepoActResult.SuccessRepoAct();
    }

    public async Task<ICollection<FinishedApplication>> GetAllFinishedApplicationsAsync()
    {
        return await applicationContext.FinishedApplications.ToListAsync();
    }
}