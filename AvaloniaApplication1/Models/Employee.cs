using System;

namespace AvaloniaApplication1.Models;

public class Employee
{
    public int Id { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string Position { get; set; }
    
    public DateTime HireDate { get; set; }
    
    public bool IsActive { get; set; }
}