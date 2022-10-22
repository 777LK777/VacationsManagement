using FluentValidation;
using VacationService.DTO.Vacations;

namespace VacationService.Validators.Vacations;

public class GetVacationsByTeamRequestValidator : AbstractValidator<GetVacationsByTeamRequest>
{
    public GetVacationsByTeamRequestValidator()
    {
        RuleFor(r => r)
            .NotEmpty()
            .WithMessage("EmployeeIds list is null or empty");

        RuleFor(r => r.EmployeesIds.FirstOrDefault(id => Guid.Empty == id))
            .Null()
            .WithMessage("EmployeeIds list contain is null or empty employeeId");
    }
    
    
}