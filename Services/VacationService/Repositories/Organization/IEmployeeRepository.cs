using VacationService.DTO.Organization;

namespace VacationService.Repositories.Organization;

public interface IEmployeeRepository
{
    Task CreateAsync(Employee employee);
    Task<Employee> GetEmployeeByIdAsync(Guid employeeId);
    Task<List<Guid>> GetEmployeesIdsByTeamIdAsync(Guid teamId);
}