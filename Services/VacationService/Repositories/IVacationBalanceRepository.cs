using VacationService.DTO.Vacations;

namespace VacationService.Repositories;

public interface IVacationBalanceRepository
{
    Task<bool> Create(VacationBalance vacationBalance);
    Task<bool> Update(VacationBalance vacationBalance);
    Task<VacationBalance> GetVacationBalanceByEmployeeId(Guid employeeId);
}