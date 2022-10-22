using VacationService.DTO.Organization;
using VacationService.DB.Models.Organization;

namespace VacationService.Mappers.Implementation.Organization;

public class EmployeeToDbEmployeeMapper : IMapper<Employee, DbEmployee>
{
    public DbEmployee Map(Employee dbVacation)
        => new()
        {
            Id = dbVacation.Id,
            TeamId = dbVacation.TeamId,
            FirstName = dbVacation.FirstName,
            LastName = dbVacation.LastName
        };
}