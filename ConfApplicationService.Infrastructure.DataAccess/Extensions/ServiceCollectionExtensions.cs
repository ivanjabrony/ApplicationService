using ConfApplicationService.Abstractions.Repositories;
using ConfApplicationService.Implementation.DataContext;
using ConfApplicationService.Implementation.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ConfApplicationService.Implementation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureDataAccess(this IServiceCollection collection)
    {
        collection.AddScoped<IUserRepository, UserRepository>();
        collection.AddScoped<IApplicationRepository, ApplicationRepository>();
        collection.AddScoped<IDraftRepository, DraftRepository>();
        
        collection.AddDbContext<ApplicationServiceContext>(
            opts =>
            {
                opts.UseNpgsql(
                    @"Server=localhost;Port=5432;Database=Application-database;User ID=postgres;Password=postgres");
            });
        
        return collection;
    }
}