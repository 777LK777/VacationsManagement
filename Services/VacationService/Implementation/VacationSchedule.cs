using System.Globalization;
using System.Text;
using VacationService.DTO.Organization.Requests;
using VacationService.DTO.Vacations;
using VacationService.Repositories;

namespace VacationService.Implementation;

public class VacationSchedule : IVacationSchedule
{

    // Вдруг у сотрудника есть банк отпусков
    
    // Сервис должен быть подписан на момент создания сущности Employee
    
    // Исходим из той гипотезы что за 365 дней полагается 28 дней отпуска
    // Если сотрудник устроился в середине года, он может получить 14 дней отпуска
    
    // Если он не использовал дни отпуска в прошлом году, то оставшиеся дни
    // переносятся на следующий год
    
    // Пополнение баланса отпускных дней производится 01 января в 00:00
    // При запуске сервиса происходит проверка, были ли проведены перерасчеты

    private readonly IVacationRepository _vacationRepository;
    private readonly IVacationCheckConditions _vacationConditions;
    private readonly IVacationBalanceRepository _vacationBalanceRepository;
    
    public VacationSchedule(
        IVacationRepository vacationRepository,
        IVacationCheckConditions vacationConditions,
        IVacationBalanceRepository vacationBalanceRepository)
    {
        _vacationRepository = vacationRepository;
        _vacationConditions = vacationConditions;
        _vacationBalanceRepository = vacationBalanceRepository;
    }
    
    public async Task<bool> AddVacationAsync(Vacation vacation)
    {
        var errorMessage = new StringBuilder(2);

        if (!await _vacationConditions.CheckWithoutIntersectionsAsync(vacation))
            errorMessage.AppendLine("Дни отпуска пересекаются с отпуском коллег");
        if (!_vacationConditions.CheckOkVacationMinTime(vacation))
            errorMessage.AppendLine("Продолжительность отпуска должна быть более 7 дней");
        if (!_vacationConditions.CheckOkVacationMaxTime(vacation))
            errorMessage.AppendLine("Продолжительность отпуска должна быть менее 14 дней");
        if (!await _vacationConditions.CheckTotalVacationTimeAsync(vacation))
            errorMessage.AppendLine("Суммарная продолжительность отпусков за год более выделенного срока");
        if (!await _vacationConditions.CheckVacationTimeTwoWeeksLeastOnceAsync(vacation))
            errorMessage.AppendLine("У сотрудника отпуск должен быть минимум один раз 14 дней");

        if (errorMessage.Length > 0)
            throw new ArgumentOutOfRangeException(errorMessage.ToString());

        var balance = await _vacationBalanceRepository.GetVacationBalanceByEmployeeId(vacation.EmployeeId);

        balance.Days -= vacation.Days;

        return await SaveVacation(vacation) && await UpdateVacationBalance(balance);
    }

    public async Task<List<Vacation>> GetVacationsByEmployee(Guid employeeId)
    {
        return await _vacationRepository.GetVacationsByEmployeeIdAsync(employeeId);
    }

    public async Task<List<Vacation>> GetVacationsByEmployees(List<Guid> employeesIds)
    {
        return await _vacationRepository.GetVacationsByEmployeesIdsAsync(employeesIds);
    }

    public async Task OnEmployeeCreate(OnEmployeeCreateData onEmployeeCreateData)
    {
        var calendar = new GregorianCalendar();
        var date = onEmployeeCreateData.DateOfEmployment;
        
        var vacationDayByWorkDay = 28.0 / calendar.GetDaysInYear(date.Year);
        var vacationDaysForRestOfYear = vacationDayByWorkDay - date.DayOfYear + 1;
        
        var vacationBalance = new VacationBalance
        {
            Id = Guid.NewGuid(),
            Days = RoundDays(vacationDaysForRestOfYear),
            EmployeeId = onEmployeeCreateData.EmployeeId,
            CurrentYear = onEmployeeCreateData.DateOfEmployment.Year,
        };

        await _vacationBalanceRepository.Create(vacationBalance);
    }

    public async Task OnEmployeeDismissed(OnEmployeeDismissedData onEmployeeDismissedData)
    {
        throw new NotImplementedException();
    }

    private int RoundDays(double days)
    {
        return (int)Math.Round(days, MidpointRounding.ToPositiveInfinity);
    }

    private async Task<bool> UpdateVacationBalance(VacationBalance vacationBalance)
    {
        return await _vacationBalanceRepository.Update(vacationBalance);
    }

    private async Task<bool> SaveVacation(Vacation vacation)
    {
        return await _vacationRepository.CreateAsync(vacation);
    }
    
    



}