namespace VacationService.DTO.Organization.Requests;

public class OnEmployeeCreateData
{
    public Guid EmployeeId { get; set; }
    public DateOnly DateOfEmployment { get; set; }
}