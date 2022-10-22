using VacationService.DTO.Organization;

namespace VacationService.Repositories.Organization;

public interface ITeamRepository
{
    Task Create(Team team);
}