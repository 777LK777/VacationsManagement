namespace VacationService.DTO.Vacations;

public class CreateVacationRequest
{
    public Guid EmployeeId { get; set; }
    public DateOnly DateStart { get; set; }
    public int Days { get; set; }
}