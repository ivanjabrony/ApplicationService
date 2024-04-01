using ConfApplicationService.Models.ResultTypes;

namespace ConfApplicationService.Contracts.Users;

public interface IUserService
{
    Task<ActionResultType> RegisterUserAsync(string name, Guid userId = new());
    Task<ActionResultType> LoginUserAsync(Guid userId);
    Task<ActionResultType> DeleteUserAsync(Guid userId);
}