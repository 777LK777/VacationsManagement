using FluentValidation;

using VacationService.Repositories;
using VacationService.DTO.Vacations;

namespace VacationService.Commands.Implementations;

public class CreateVacationCommand : ICommand<CreateVacationRequest, bool>
{
    private readonly IValidator<CreateVacationRequest> _validator;
    private readonly IVacationRepository _vacationRepository;
    private readonly IVacationSchedule _vacationSchedule;

    public CreateVacationCommand(
        IValidator<CreateVacationRequest> validator,
        IVacationRepository vacationRepository,
        IVacationSchedule vacationSchedule)
    {
        _validator = validator;
        _vacationRepository = vacationRepository;
        _vacationSchedule = vacationSchedule;
    }
    
    public async Task<bool> Execute(CreateVacationRequest request)
    {
        _validator.ValidateAndThrow(request);

        var vacation = new Vacation()
        {
            EmployeeId = request.EmployeeId,
            DateStart = request.DateStart,
            Days = request.Days
        };

        return await _vacationSchedule.AddVacationAsync(vacation);
    }
}