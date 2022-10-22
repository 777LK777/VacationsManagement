using VacationService.DB.Models;
using VacationService.DTO.Vacations;
using VacationService.Mappers;

namespace VacationService.Repositories.Implementation;

public class VacationRepository : IVacationRepository
{
    private readonly List<DbVacation> _store;
    private readonly IMapper<Vacation, DbVacation> _toDbMapper;
    private readonly IMapper<DbVacation, Vacation> _fromDbMapper;

    public VacationRepository(
        IMapper<Vacation, DbVacation> toDbMapper,
        IMapper<DbVacation, Vacation> fromDbMapper)
    {
        _store = new List<DbVacation>();
        _toDbMapper = toDbMapper;
        _fromDbMapper = fromDbMapper;
    }

    public async Task<bool> CreateAsync(Vacation vacation)
    {
        _store.Add(_toDbMapper.Map(vacation));
        return true;
    }

    public async Task<List<Vacation>> GetVacationsByEmployeesIdsAsync(List<Guid> employeesIds)
    {
        var dbVacations = _store
            .Where(dbv => employeesIds.Exists(eId => eId == dbv.EmployeeId));
        var result = new List<Vacation>();

        foreach (var dbVacation in dbVacations)
            result.Add(_fromDbMapper.Map(dbVacation));
        
        return result;
    }

    public async Task<List<Vacation>> GetVacationsByEmployeeIdAsync(Guid employeeId)
    {
        var dbVacations = _store.Where(dbv => dbv.EmployeeId == employeeId);
        var result = new List<Vacation>();

        foreach (var dbVacation in dbVacations)
            result.Add(_fromDbMapper.Map(dbVacation));
        
        return result;
    }
}