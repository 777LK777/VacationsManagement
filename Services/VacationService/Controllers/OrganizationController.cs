using Microsoft.AspNetCore.Mvc;
using VacationService.Commands;
using VacationService.DTO.Organization.Requests;

namespace VacationService.Controllers;

// Organization structure
// Department -> Team -> Employee

[ApiController]
[Route("[controller]")]
public class OrganizationController : ControllerBase
{
    private readonly ILogger<OrganizationController> _logger;

    public OrganizationController(ILogger<OrganizationController> logger)
    {
        _logger = logger;
    }

    [Route("hireEmployee")]
    [HttpPost]
    public async Task<bool> HireEmployee(
        [FromServices] ICommand<HireEmployeeRequest, bool> command,
        [FromBody] HireEmployeeRequest request)
    {
        _logger.LogInformation("Hire employee http request received from client to VacationService");
        return await command.Execute(request);
    }

    [Route("transferEmployeeToOtherTeam")]
    [HttpPost]
    public async Task<bool> TransferEmployeeToOtherTeam(
        [FromServices] ICommand<TransferEmployeeToOtherTeamRequest, bool> command,
        [FromBody] TransferEmployeeToOtherTeamRequest request)
    {
        _logger.LogInformation("Transfer employee to other team http request received from client to VacationService");
        return await command.Execute(request);
    }

    [Route("dismissEmployee")]
    [HttpPost]
    public async Task<bool> DismissEmployee(
        [FromServices] ICommand<DismissEmployeeRequest, bool> commmand,
        [FromBody] DismissEmployeeRequest request)
    {
        _logger.LogInformation("Dismiss employee http request received from client to VacationService");
        return await commmand.Execute(request);
    }

    [Route("createTeam")]
    [HttpPost]
    public async Task<bool> CreateTeam(
        [FromServices] ICommand<CreateTeamRequest, bool> command,
        [FromBody] CreateTeamRequest createTeamRequest)
    {
        _logger.LogInformation("Create team http request received from client to VacationService");
        return await command.Execute(createTeamRequest);
    }
}