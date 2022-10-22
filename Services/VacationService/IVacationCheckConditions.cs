using VacationService.DTO.Vacations;

namespace VacationService;

public interface IVacationCheckConditions
{
    bool CheckOkVacationMinTime(Vacation vacation);
    bool CheckOkVacationMaxTime(Vacation vacation);
    Task<bool> CheckWithoutIntersectionsAsync(Vacation vacation);
    Task<bool> CheckTotalVacationTimeAsync(Vacation vacation);
    Task<bool> CheckVacationTimeTwoWeeksLeastOnceAsync(Vacation vacation);
}