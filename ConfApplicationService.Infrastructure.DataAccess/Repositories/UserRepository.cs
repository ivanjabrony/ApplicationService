using ConfApplicationService.Abstractions.Repositories;
using ConfApplicationService.Implementation.DataContext;
using ConfApplicationService.Models.ResultTypes;
using ConfApplicationService.Models.Users;

namespace ConfApplicationService.Implementation.Repositories;

public class UserRepository(ApplicationServiceContext applicationContext) : IUserRepository
{
    public async Task<User?> FindUserAsync(Guid userId)
    {
        return await applicationContext.Users.FindAsync(userId);
    }

    public async Task<User?> CreateUserAsync(string userName, Guid userId = new())
    {
        var userEntity = await applicationContext.Users.AddAsync(new User(userName, userId));
        await applicationContext.SaveChangesAsync();
        return userEntity.Entity;
    }

    public async Task<RepoActResult> DeleteUserAsync(Guid userId)
    {
        var user = await applicationContext.Users.FindAsync(userId);
        if (user == null) return new RepoActResult.FailRepoAct("Couldn't find user with such name");
        
        applicationContext.Users.Remove(user);
        await applicationContext.SaveChangesAsync();
        return new RepoActResult.SuccessRepoAct();
    }
}