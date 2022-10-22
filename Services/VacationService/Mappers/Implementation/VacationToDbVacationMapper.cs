using VacationService.DB.Models;
using VacationService.DTO.Vacations;

namespace VacationService.Mappers.Implementation;

public class VacationToDbVacationMapper : IMapper<Vacation, DbVacation>
{
    public DbVacation Map(Vacation vacation)
    {
        return new DbVacation
        {
            Id = vacation.Id,
            Days = vacation.Days,
            DateStart = vacation.DateStart,
            EmployeeId = vacation.EmployeeId
        };
    }
}