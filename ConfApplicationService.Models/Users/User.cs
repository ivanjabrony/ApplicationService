using System.Collections;
using ConfApplicationService.Models.Applications;

namespace ConfApplicationService.Models.Users;

public record User(
    string Name,
    Guid UserId = new()
    );