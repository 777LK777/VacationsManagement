namespace VacationService.DB.Models;

public class DbVacation
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public DateOnly DateStart { get; set; }
    public int Days { get; set; }
}