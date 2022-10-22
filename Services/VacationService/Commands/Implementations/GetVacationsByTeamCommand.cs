using FluentValidation;
using VacationService.DTO.Vacations;

namespace VacationService.Commands.Implementations;

public class GetVacationsByTeamCommand : ICommand<GetVacationsByTeamRequest, GetVacationsByTeamResponse>
{
    private readonly IVacationSchedule _vacationSchedule;
    private readonly IValidator<GetVacationsByTeamRequest> _validator;

    public GetVacationsByTeamCommand(
        IValidator<GetVacationsByTeamRequest> validator,
        IVacationSchedule vacationSchedule)
    {
        _validator = validator;
        _vacationSchedule = vacationSchedule;
    }
    public async Task<GetVacationsByTeamResponse> Execute(GetVacationsByTeamRequest request)
    {
        _validator.ValidateAndThrow(request);
        var vacations = await _vacationSchedule.GetVacationsByEmployees(request.EmployeesIds);
        
        var response = new GetVacationsByTeamResponse
        {
            Vacations = vacations
        };
        
        return response;
    }
}