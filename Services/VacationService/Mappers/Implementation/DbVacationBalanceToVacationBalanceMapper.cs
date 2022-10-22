using VacationService.DB.Models;
using VacationService.DTO.Vacations;

namespace VacationService.Mappers.Implementation;

public class DbVacationBalanceToVacationBalanceMapper : IMapper<DbVacationBalance, VacationBalance>
{
    public VacationBalance Map(DbVacationBalance dbVacationBalance)
    {
        return new VacationBalance
        {
            Id = dbVacationBalance.Id,
            Days = dbVacationBalance.Days,
            EmployeeId = dbVacationBalance.EmployeeId,
            CurrentYear = dbVacationBalance.CurrentYear,
        };
    }
}