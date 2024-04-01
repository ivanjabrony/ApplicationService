using ConfApplicationService.Contracts.Users;
using ConfApplicationService.Models.Users;

namespace ConfApplicationService.Users;

public class CurrentUserManager : ICurrentUserService
{
    public User? User { get; set; }
}