using VacationService.DB.Contexts;

using Microsoft.EntityFrameworkCore;
using VacationService.DB.Migrations;

namespace VacationService.DB;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDB(this IServiceCollection services, IConfiguration configuration)
    {
        var migrationEngine = new MigrationEngine(configuration);
        migrationEngine.Migrate();
        
        return services
            .AddDbContext<OrganizationDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("OrganizationConnectionString"));
            })
            .AddDbContext<VacationManagementDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("VacationManagementConnectionString"));
            });
    }
}