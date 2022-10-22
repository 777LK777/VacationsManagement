using VacationService.DTO.Organization.Requests;
using VacationService.DTO.Vacations;

namespace VacationService;

/// <summary>
/// График отпусков
/// </summary>
public interface IVacationSchedule
{
    Task<bool> AddVacationAsync(Vacation vacation);
    Task<List<Vacation>> GetVacationsByEmployee(Guid employeeId);
    Task<List<Vacation>> GetVacationsByEmployees(List<Guid> employeesIds);
    Task OnEmployeeCreate(OnEmployeeCreateData onEmployeeCreateData);
    Task OnEmployeeDismissed(OnEmployeeDismissedData onEmployeeDismissedData);
}