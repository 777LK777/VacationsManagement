namespace VacationService.DB.Models;

public class DbVacationBalance
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public int Days { get; set; }
    public int CurrentYear { get; set; }
}