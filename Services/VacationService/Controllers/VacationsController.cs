using Microsoft.AspNetCore.Mvc;

using VacationService.Commands;
using VacationService.DTO.Vacations;

namespace VacationService.Controllers;

[ApiController]
[Route("[controller]")]
public class VacationsController : ControllerBase
{
    private readonly ILogger<VacationsController> _logger;

    public VacationsController(ILogger<VacationsController> logger)
    {
        _logger = logger;
    }

    [Route("getVacationsByTeam")]
    [HttpGet]
    public async Task<GetVacationsByTeamResponse> GetVacationsByTeam(
        [FromServices] ICommand<List<Guid>, GetVacationsByTeamResponse> command,
        [FromHeader] List<Guid> employeesInTeam)
    {
        _logger.LogInformation("Get vacations by team http request received from client to VacationService");
        return await command.Execute(employeesInTeam);
    }

    [Route("getVacationsByEmployee")]
    [HttpGet]
    public async Task<GetVacationsByEmployeeIdResponse> GetVacationsByEmployeeId(
        [FromServices] ICommand<Guid, GetVacationsByEmployeeIdResponse> command,
        [FromHeader] Guid employeeId)
    {
        _logger.LogInformation("Get vacations by employee http request received from client to VacationService");
        return await command.Execute(employeeId);
    }

    [Route("create")]
    [HttpPost]
    public async Task<bool> CreateVacation(
        [FromServices] ICommand<CreateVacationRequest, bool> command,
        [FromBody] CreateVacationRequest request)
    {
        _logger.LogInformation("Create vacation http request received from client to VacationService");
        return await command.Execute(request);
    }

    [Route("move")]
    [HttpPost]
    public async Task<bool> MoveVacation(
        [FromServices] ICommand<MoveVacationRequest, bool> command,
        [FromBody] MoveVacationRequest request)
    {
        _logger.LogInformation("Move vacation http request received from client to VacationService");
        return await command.Execute(request);
    }

    [Route("remove")]
    [HttpPost]
    public async Task<bool> RemoveVacation(
        [FromServices] ICommand<RemoveVacationRequest, bool> command,
        [FromBody] RemoveVacationRequest request)
    {
        _logger.LogInformation("Remove vacation http request received from client to VacationService");
        return await command.Execute(request);
    }
}