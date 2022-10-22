using VacationService.DTO.Vacations;
using VacationService.Repositories.Implementation;
using VacationService.Repositories.Organization;
using VacationService.Repositories.Organization.Implementation;

namespace VacationService.Repositories;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services
            // Organization
            .AddSingleton<ITeamRepository, TeamRepository>()
            .AddSingleton<IEmployeeRepository, EmployeeRepository>()
            
            // Vacations
            .AddSingleton<IVacationBalanceRepository, VacationBalanceRepository>()
            .AddSingleton<IVacationRepository, VacationRepository>();
        
        return services;
    }
}