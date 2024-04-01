using ConfApplicationService.Applications;
using ConfApplicationService.Contracts.Applications;
using ConfApplicationService.Contracts.Users;
using ConfApplicationService.Users;
using Microsoft.Extensions.DependencyInjection;

namespace ConfApplicationService.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection collection)
    {
        collection.AddScoped<IUserService, UserService>();
        collection.AddScoped<CurrentUserManager>();
        collection.AddScoped<ICurrentUserService>(
            p => p.GetRequiredService<CurrentUserManager>());
        
        collection.AddScoped<IApplicationService, ApplicationService>();
        collection.AddScoped<CurrentDraftEditor>();
        collection.AddScoped<IDraftEditor>(
            p => p.GetRequiredService<CurrentDraftEditor>());
        
        return collection;
    }
}