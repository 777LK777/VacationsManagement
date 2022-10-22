using VacationService.DB.Models;
using VacationService.DTO.Vacations;

namespace VacationService.Mappers.Implementation;

public class VacationBalanceToDbVacationBalanceMapper : IMapper<VacationBalance, DbVacationBalance>
{
    public DbVacationBalance Map(VacationBalance vacationBalance)
    {
        return new DbVacationBalance()
        {
            Id = vacationBalance.Id,
            Days = vacationBalance.Days,
            EmployeeId = vacationBalance.EmployeeId,
            CurrentYear = vacationBalance.CurrentYear,
        };
    }
}