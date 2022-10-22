using VacationService.DTO.Vacations;

namespace VacationService.Repositories;

public interface IVacationRepository
{
    Task<bool> CreateAsync(Vacation vacation);
    Task<List<Vacation>> GetVacationsByEmployeesIdsAsync(List<Guid> employeesIds);
    Task<List<Vacation>> GetVacationsByEmployeeIdAsync(Guid employeeId);
}