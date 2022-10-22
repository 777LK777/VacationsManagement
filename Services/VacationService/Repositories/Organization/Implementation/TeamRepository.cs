using VacationService.DB.Contexts;
using VacationService.Mappers;
using VacationService.DTO.Organization;
using VacationService.DB.Models.Organization;

namespace VacationService.Repositories.Organization.Implementation;

public class TeamRepository : ITeamRepository
{
    private readonly IMapper<Team, DbTeam> _teamToDbToMapper;
    private readonly OrganizationDbContext _organizationDbContext;

    public TeamRepository(
        IMapper<Team, DbTeam> mapper,
        OrganizationDbContext organizationDbContext)
    {
        _teamToDbToMapper = mapper;
        _organizationDbContext = organizationDbContext;
    }
    
    public async Task Create(Team team)
    {
        await _organizationDbContext.Teams.AddAsync(_teamToDbToMapper.Map(team));
        await _organizationDbContext.SaveChangesAsync();
    }
}