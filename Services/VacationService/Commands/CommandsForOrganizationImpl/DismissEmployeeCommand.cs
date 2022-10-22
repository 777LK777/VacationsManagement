using VacationService.DTO.Organization.Requests;

namespace VacationService.Commands.CommandsForOrganizationImpl;

public class DismissEmployeeCommand : ICommand<DismissEmployeeRequest, bool>
{
    public Task<bool> Execute(DismissEmployeeRequest request)
    {
        throw new NotImplementedException();
    }
}