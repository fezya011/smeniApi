using System;

namespace AvaloniaApplication1.Models;

public class Shift
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string Description { get; set; }
    
    public Employee Employee { get; set; }
}