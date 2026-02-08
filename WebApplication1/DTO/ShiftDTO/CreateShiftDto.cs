namespace WebApplication1.DTO.ShiftDTO;

public class CreateShiftDto
{
    public int EmployeeId { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string Description { get; set; }
}