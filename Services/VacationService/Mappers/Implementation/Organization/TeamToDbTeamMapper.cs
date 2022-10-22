using VacationService.DTO.Organization;
using VacationService.DB.Models.Organization;

namespace VacationService.Mappers.Implementation.Organization;

public class TeamToDbTeamMapper : IMapper<Team, DbTeam>
{
    public DbTeam Map(Team dbVacation)
    {
        return new DbTeam()
        {
            Id = dbVacation.Id,
            Title = dbVacation.Title
        };
    }
}