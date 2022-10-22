using VacationService.Commands;
using VacationService.Commands.Implementations;
using VacationService.Commands.CommandsForOrganizationImpl;

using VacationService.DTO.Vacations;
using VacationService.DTO.Organization.Requests;

namespace VacationService.Commands;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommands(
        this IServiceCollection services)
    {
        services
            // Organization
            .AddTransient<ICommand<CreateTeamRequest>, CreateTeamCommand>()
            .AddTransient<ICommand<HireEmployeeRequest>, CreateEmployeeCommand>()
            .AddTransient<ICommand<DismissEmployeeRequest, bool>, DismissEmployeeCommand>()

            // Vacations
            .AddTransient<ICommand<CreateVacationRequest, bool>, CreateVacationCommand>()
            .AddTransient<ICommand<GetVacationsByTeamRequest, GetVacationsByTeamResponse>, GetVacationsByTeamCommand>();
        
        return services;
    }
}