using ConfApplicationService.Abstractions.Repositories;
using ConfApplicationService.Contracts.Users;
using ConfApplicationService.Models.ResultTypes;

namespace ConfApplicationService.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly CurrentUserManager _currentUserManager;
    public UserService(IUserRepository repository, CurrentUserManager userManager)
    {
        _repository = repository;
        _currentUserManager = userManager;
    }

    public async Task<ActionResultType> RegisterUserAsync(string name, Guid userId = new Guid())
    {
        var user = await _repository.CreateUserAsync(name, userId).ConfigureAwait(false);
        if (user == null) return new RepoActResult.FailRepoAct("Error: couldn't register profile");
        return new RepoActResult.SuccessRepoAct();
    }

    public async Task<ActionResultType> LoginUserAsync(Guid userId)
    {
        var user = await _repository.FindUserAsync(userId);
        if (user == null) return new RepoActResult.FailRepoAct("Error: couldn't register profile");
        _currentUserManager.User = user;
        return new RepoActResult.SuccessRepoAct();
    }

    public async Task<ActionResultType> DeleteUserAsync(Guid userId)
    {
        return await _repository.DeleteUserAsync(userId);
    }
}