using FluentValidation;

using VacationService.DTO.Vacations;

namespace VacationService.Validators.Vacations;

public class CreateVacationRequestValidator : AbstractValidator<CreateVacationRequest>
{
    public CreateVacationRequestValidator()
    {
        RuleFor(vacation => vacation.EmployeeId)
            .NotEmpty()
            .WithMessage("UserId is null or empty");
    }
}