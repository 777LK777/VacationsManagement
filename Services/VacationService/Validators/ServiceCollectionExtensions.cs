using FluentValidation;

using VacationService.DTO.Vacations;
using VacationService.DTO.Organization.Requests;
using VacationService.Validators.Vacations;
using VacationService.Validators.Organization;

namespace VacationService.Validators;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services

            // Organization
            .AddTransient<IValidator<CreateTeamRequest>, CreateTeamRequestValidator>()
            .AddTransient<IValidator<HireEmployeeRequest>, HireEmployeeRequestValidator>()

            // Vacations
            .AddTransient<IValidator<CreateVacationRequest>, CreateVacationRequestValidator>()
            .AddTransient<IValidator<GetVacationsByTeamRequest>, GetVacationsByTeamRequestValidator>()
            .AddTransient<IValidator<GetVacationsByEmployeeRequest>, GetVacationsByEmployeeRequestValidator>();
        
        return services;
    }
}