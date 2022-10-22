namespace VacationService.DTO.Organization.Requests;

public class HireEmployeeRequest
{
    public Guid TeamId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly DateOfEmployment { get; set; }
}