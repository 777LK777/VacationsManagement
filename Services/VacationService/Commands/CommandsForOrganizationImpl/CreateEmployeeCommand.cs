using FluentValidation;

using VacationService.DTO.Organization;
using VacationService.DTO.Organization.Requests;
using VacationService.Repositories.Organization;

namespace VacationService.Commands.CommandsForOrganizationImpl;

public class CreateEmployeeCommand : ICommand<HireEmployeeRequest>
{
    private readonly IVacationSchedule _vacationSchedule;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IValidator<HireEmployeeRequest> _validator;

    public CreateEmployeeCommand(
        IVacationSchedule vacationSchedule,
        IEmployeeRepository employeeRepository,
        IValidator<HireEmployeeRequest> validator)
    {
        _validator = validator;
        _vacationSchedule = vacationSchedule;
        _employeeRepository = employeeRepository;
    }
    
    public async Task Execute(HireEmployeeRequest request)
    {
        await _validator.ValidateAndThrowAsync(request);

        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            TeamId = request.TeamId,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        await CreateEmployee(employee);

        var savedEmployee = await _employeeRepository.GetEmployeeByIdAsync(employee.Id);
        
        if (savedEmployee != null) await Notify(employee.Id, request.DateOfEmployment);
    }

    private async Task CreateEmployee(
        Employee employee)
    {
        await _employeeRepository.CreateAsync(employee);
    }

    private async Task Notify(Guid employeeId, DateOnly dateOfEmployment)
    {
        var createData = new OnEmployeeCreateData
        {
            EmployeeId = employeeId,
            DateOfEmployment = dateOfEmployment
        };
        
        await _vacationSchedule.OnEmployeeCreate(createData);
    }
}