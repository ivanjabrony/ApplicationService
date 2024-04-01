using ConfApplicationService.Models.Users;

namespace ConfApplicationService.Contracts.Users;

public interface ICurrentUserService
{
    User? User { get; set; }
}