using ConfApplicationService.Models.ResultTypes;
using ConfApplicationService.Models.Users;

namespace ConfApplicationService.Abstractions.Repositories;

public interface IUserRepository
{
    Task<User?> FindUserAsync(Guid userId);
    Task<User?> CreateUserAsync(string userName, Guid userId);
    Task<RepoActResult> DeleteUserAsync(Guid userId);
}