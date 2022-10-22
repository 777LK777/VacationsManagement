using FluentValidation;
using VacationService.DTO.Vacations;

namespace VacationService.Validators.Vacations;

public class GetVacationsByEmployeeRequestValidator : AbstractValidator<GetVacationsByEmployeeRequest>
{
    public GetVacationsByEmployeeRequestValidator()
    {
        RuleFor(r => r.EmployeeId)
            .NotEmpty()
            .WithMessage("EmployeeId is null or empty");
    }
}