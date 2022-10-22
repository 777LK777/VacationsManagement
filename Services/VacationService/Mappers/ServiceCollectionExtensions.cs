using VacationService.DB.Models;
using VacationService.DTO.Organization;
using VacationService.DB.Models.Organization;
using VacationService.DTO.Vacations;
using VacationService.Mappers.Implementation;
using VacationService.Mappers.Implementation.Organization;

namespace VacationService.Mappers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        services 
            // Organization
            .AddTransient<IMapper<Team, DbTeam>, TeamToDbTeamMapper>()
            .AddTransient<IMapper<Employee, DbEmployee>, EmployeeToDbEmployeeMapper>()
            .AddTransient<IMapper<DbEmployee, Employee>, DbEmployeeToEmployeeMapper>()
            
            // Vacations
            .AddTransient<IMapper<VacationBalance, DbVacationBalance>, VacationBalanceToDbVacationBalanceMapper>()
            .AddTransient<IMapper<DbVacationBalance, VacationBalance>, DbVacationBalanceToVacationBalanceMapper>()
            .AddTransient<IMapper<Vacation, DbVacation>, VacationToDbVacationMapper>()
            .AddTransient<IMapper<DbVacation, Vacation>, DbVacationToVacationMapper>();

        return services;
    }
}