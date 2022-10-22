namespace VacationService.DTO.Organization;

public class Employee
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}