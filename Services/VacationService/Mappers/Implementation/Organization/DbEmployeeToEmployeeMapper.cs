using VacationService.DTO.Organization;
using VacationService.DB.Models.Organization;

namespace VacationService.Mappers.Implementation.Organization;

public class DbEmployeeToEmployeeMapper : IMapper<DbEmployee, Employee>
{
    public Employee Map(DbEmployee dbEmployee)
        => new()
        {
            Id = dbEmployee.Id,
            TeamId = dbEmployee.TeamId,
            FirstName = dbEmployee.FirstName,
            LastName = dbEmployee.LastName
        };
}