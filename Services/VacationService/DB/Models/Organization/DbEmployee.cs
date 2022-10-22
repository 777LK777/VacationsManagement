namespace VacationService.DB.Models.Organization;

public class DbEmployee
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}