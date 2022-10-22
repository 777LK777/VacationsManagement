using FluentValidation;

using VacationService.DTO.Organization.Requests;

namespace VacationService.Validators.Organization;

public class CreateTeamRequestValidator : AbstractValidator<CreateTeamRequest>
{
    public CreateTeamRequestValidator()
    {
        RuleFor(request => request.Title)
            .NotEmpty()
            .WithMessage("Team title must not be empty")
            .MaximumLength(50)
            .WithMessage("Team title is too long")
            .MinimumLength(2)
            .WithMessage("Team title is too short")
            .Matches(@"^[A-Z][\w -]+$")
            .WithMessage("The first character must be uppercase letter and the rest must be lowercase. " +
                         "Team title must contain only letters, numbers and dashes");
    }
}