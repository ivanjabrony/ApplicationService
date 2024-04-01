using ConfApplicationService.Abstractions.Repositories;
using ConfApplicationService.Implementation.DataContext;
using ConfApplicationService.Models.Applications;
using ConfApplicationService.Models.ResultTypes;
using Microsoft.EntityFrameworkCore;

namespace ConfApplicationService.Implementation.Repositories;

public class DraftRepository(ApplicationServiceContext applicationContext) : IDraftRepository
{
    public async Task<DraftApplication?> CreateNewDraftForUserAsync(Guid userId)
    {
        var draft = new DraftApplication
        {
            AuthorId = userId
        };
        applicationContext.DraftApplications.Add(draft);
        await applicationContext.SaveChangesAsync();
        return await applicationContext.DraftApplications.FindAsync(draft.DraftApplicationId);

    }

    public async Task<DraftApplication?> GetDraftOfUserAsync(Guid userId)
    {
        return await applicationContext.DraftApplications.SingleOrDefaultAsync(x => x.AuthorId.Equals(userId));
    }

    public async Task<RepoActResult> DeleteDraftOfUserAsync(Guid userId)
    {
        var draft = await applicationContext.DraftApplications.SingleOrDefaultAsync(x => x.AuthorId.Equals(userId));
        if (draft == null) return new RepoActResult.FailRepoAct("Couldn't find any drafts of that user");

        applicationContext.DraftApplications.Remove(draft);
        await applicationContext.SaveChangesAsync();

        return new RepoActResult.SuccessRepoAct();
    }

    public async Task<ICollection<DraftApplication>> GetAllDraftsAsync()
    {
        return await applicationContext.DraftApplications.ToListAsync();
    }

    public async Task<RepoActResult> SaveDraftChanges(Guid userId)
    {
        var result = await applicationContext.DraftApplications.FindAsync(userId);
        if (result == null) return new RepoActResult.FailRepoAct("Couldn't get draft of user");
        await applicationContext.SaveChangesAsync();
        return new RepoActResult.SuccessRepoAct();
    }
}