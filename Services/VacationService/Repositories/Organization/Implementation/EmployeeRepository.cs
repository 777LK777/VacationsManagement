using Microsoft.EntityFrameworkCore;
using VacationService.Mappers;
using VacationService.DB.Contexts;
using VacationService.DB.Models.Organization;
using VacationService.DTO.Organization;

namespace VacationService.Repositories.Organization.Implementation;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly IMapper<Employee, DbEmployee> _toDbMapper;
    private readonly IMapper<DbEmployee, Employee> _fromDbMapper;
    private readonly OrganizationDbContext _organizationDbContext;

    public EmployeeRepository(
        IMapper<Employee, DbEmployee> toDbMapper,
        IMapper<DbEmployee, Employee> fromDbMapper,
        OrganizationDbContext organizationDbDbContext)
    {
        _toDbMapper = toDbMapper;
        _fromDbMapper = fromDbMapper;
        _organizationDbContext = organizationDbDbContext;
    }
    
    public async Task CreateAsync(Employee employee)
    {
        await _organizationDbContext.Employees
            .AddAsync(_toDbMapper.Map(employee));
        await _organizationDbContext.SaveChangesAsync();
    }

    public async Task<List<Guid>> GetEmployeesIdsByTeamIdAsync(Guid teamId)
    {
        var dbIds = _organizationDbContext
            .Employees
            .Where(emp => emp.TeamId == teamId)
            .Select(emp => emp.Id);

        if (dbIds == null) return new List<Guid>();

        var ids = new List<Guid>();

        foreach (var id in dbIds)
            ids.Add(id);

        return ids;
    }

    public async Task<Employee> GetEmployeeByIdAsync(Guid employeeId)
    {
        var dbEmployee = _organizationDbContext.Employees
            .FirstOrDefault(emp => emp.Id == employeeId);
        return _fromDbMapper.Map(dbEmployee);
    }
}