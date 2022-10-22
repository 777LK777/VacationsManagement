namespace VacationService.DTO.Vacations;

public class Vacation
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public DateOnly DateStart { get; set; }
    public int Days { get; set; }
}