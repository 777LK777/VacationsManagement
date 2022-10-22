using VacationService.DB.Models;
using VacationService.DTO.Vacations;

namespace VacationService.Mappers.Implementation;

public class DbVacationToVacationMapper : IMapper<DbVacation, Vacation>
{
    public Vacation Map(DbVacation dbVacation)
    {
        return new Vacation
        {
            Id = dbVacation.Id,
            Days = dbVacation.Days,
            DateStart = dbVacation.DateStart,
            EmployeeId = dbVacation.EmployeeId
        };
    }
}