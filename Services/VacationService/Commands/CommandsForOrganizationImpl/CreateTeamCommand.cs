using FluentValidation;

using VacationService.DTO.Organization;
using VacationService.DTO.Organization.Requests;
using VacationService.Repositories.Organization;

namespace VacationService.Commands.CommandsForOrganizationImpl;

public class CreateTeamCommand : ICommand<CreateTeamRequest>
{
    private readonly ITeamRepository _teamRepository;
    private readonly IValidator<CreateTeamRequest> _validator;

    public CreateTeamCommand(
        ITeamRepository teamRepository,
        IValidator<CreateTeamRequest> validator)
    {
        _validator = validator;
        _teamRepository = teamRepository;
    }
    
    public async Task Execute(CreateTeamRequest request)
    {
        await _validator.ValidateAndThrowAsync(request);

        var team = new Team()
        {
            Id = Guid.NewGuid(),
            Title = request.Title
        };
        
        await CreateTeam(team);
    }

    private async Task CreateTeam(Team team)
    {
        await _teamRepository.Create(team);
    }
}