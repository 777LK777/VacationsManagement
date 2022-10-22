namespace VacationService.DTO.Vacations;

public class VacationBalance
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public int Days { get; set; }
    public int CurrentYear { get; set; }
}