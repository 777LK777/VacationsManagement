using VacationService.Mappers;
using VacationService.DB.Models;
using VacationService.DTO.Vacations;

namespace VacationService.Repositories.Implementation;

public class VacationBalanceRepository : IVacationBalanceRepository
{
    private readonly List<DbVacationBalance> _store;
    private readonly IMapper<VacationBalance, DbVacationBalance> _toDbMapper;
    private readonly IMapper<DbVacationBalance, VacationBalance> _fromDbMapper;

    public VacationBalanceRepository(
        IMapper<VacationBalance, DbVacationBalance> toDbMapper,
        IMapper<DbVacationBalance, VacationBalance> fromDbMapper)
    {
        _store = new List<DbVacationBalance>();
        _toDbMapper = toDbMapper;
        _fromDbMapper = fromDbMapper;
    }
    
    public async Task<bool> Create(VacationBalance vacationBalance)
    {
        _store.Add(_toDbMapper.Map(vacationBalance));
        return true;
    }

    public async Task<bool> Update(VacationBalance vacationBalance)
    {
        var vb = _store.FirstOrDefault(vb => vb.Id == vacationBalance.Id);

        vb.Days = vacationBalance.Days;
        vb.CurrentYear = vacationBalance.CurrentYear;
        
        return true;
    }

    public async Task<VacationBalance> GetVacationBalanceByEmployeeId(Guid employeeId)
    {
        // TODO: корректная проверка на null
        var dbVacationBalance = _store.FirstOrDefault(vb => vb.EmployeeId == employeeId);
        return _fromDbMapper.Map(dbVacationBalance);
    }
}