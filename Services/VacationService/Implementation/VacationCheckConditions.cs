using VacationService.DTO.Vacations;
using VacationService.Repositories;
using VacationService.Repositories.Organization;

namespace VacationService.Implementation;

public class VacationCheckConditions : IVacationCheckConditions
{
    private readonly IEmployeeRepository _employeeRepository;
    
    private readonly IVacationRepository _vacationRepository;
    private readonly IVacationBalanceRepository _vacationBalanceRepository;

    public VacationCheckConditions(
        IVacationRepository vacationRepository,
        IVacationBalanceRepository vacationBalanceRepository,
        IEmployeeRepository employeeRepository)
    {
        _vacationRepository = vacationRepository;
        _vacationBalanceRepository = vacationBalanceRepository;
        _employeeRepository = employeeRepository;
    }
    
    /// <summary>
    /// У сотрудника отпуск не должен пересекаться с отпуском коллег (на данном этапе исключений нет)
    /// </summary>
    public async Task<bool> CheckWithoutIntersectionsAsync(Vacation currentVacation)
    {
        var employee = await _employeeRepository.GetEmployeeByIdAsync(currentVacation.EmployeeId);
        var employeesIds = await _employeeRepository.GetEmployeesIdsByTeamIdAsync(employee.TeamId);
        var existingVacations = await _vacationRepository.GetVacationsByEmployeesIdsAsync(employeesIds);
        
        var intersectionNotFound = true;
        
        foreach (var existingVacation in existingVacations)
        {
            var cSt = currentVacation.DateStart; // current vacation date start
            var cEn = currentVacation.DateStart.AddDays(currentVacation.Days - 1); // current vacation date end
            var eSt = existingVacation.DateStart; // existing vacation date start
            var eEn = existingVacation.DateStart.AddDays(existingVacation.Days - 1); // existing vacation date end

            if (
                (cSt >= eSt && cSt <= eEn) // right intersection -      // existing     |_______|
                                                                        // current          |xxxx___|
                || (cEn >= eSt && cEn <= eEn) // left intersection -    // existing     |_______|
                                                                        // current  |___xxxx|
                || (cSt <=eSt && cSt <= eEn && cEn >= eEn && cEn >= eSt)) // full intersection -    // existing     |_______|
                                                                                                    // current   |___xxxxxxx____|
            {
                intersectionNotFound = false;
                break;
            }
        }

        return intersectionNotFound;
    }
    
    /// <summary>
    /// У сотрудника отпуск должен быть не дольше 28 дней
    /// </summary>
    public async Task<bool> CheckTotalVacationTimeAsync(Vacation vacation)
    {
        var vacationBalance = await _vacationBalanceRepository.GetVacationBalanceByEmployeeId(vacation.EmployeeId);
        return (vacationBalance.Days - vacation.Days) >= 0;
    }

    /// <summary>
    /// У сотрудника отпуск должен быть не короче 7 дней
    /// </summary>
    public bool CheckOkVacationMinTime(Vacation vacation)
    {
        return vacation.Days >= 7;
    }

    /// <summary>
    /// У сотрудника отпуск должен быть не больше 14 дней
    /// </summary>
    public bool CheckOkVacationMaxTime(Vacation vacation)
    {
        return vacation.Days <= 14;
    }

    /// <summary>
    /// У сотрудника отпуск должен быть минимум один раз 14 дней
    /// </summary>
    public async Task<bool> CheckVacationTimeTwoWeeksLeastOnceAsync(Vacation vacation)
    {
        var otherVacations = await _vacationRepository.GetVacationsByEmployeeIdAsync(vacation.EmployeeId);
        var vacationsBalance = await _vacationBalanceRepository.GetVacationBalanceByEmployeeId(vacation.EmployeeId);

        var daysBalance = vacationsBalance.Days - vacation.Days;

        var twoWeeksVacationCount = otherVacations.Count(v => v.Days == 14);
        if (vacation.Days == 14) twoWeeksVacationCount++;

        return !(twoWeeksVacationCount == 0 && daysBalance < 14);
    }
    
}