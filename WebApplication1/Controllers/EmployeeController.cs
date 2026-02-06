using Microsoft.AspNetCore.Mvc;
using WebApplication1.DB;
using WebApplication1.DTO;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : Controller
{
    private readonly Smeni1135Context _context;
    
    public EmployeeController(Smeni1135Context context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployeesList()
    {
          var list = _context.Employees.Select(e => new EmployeeDTO { Id = e.Id, FirstName = e.FirstName, HireDate = e.HireDate, IsActive = e.IsActive, LastName = e.LastName, Position = e.Position }).ToList();
          return Ok(list);
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployeesById(int id)
    {
        var employee = _context.Employees.Where(e => e.Id == id).Select(e => new EmployeeDTO { Id = e.Id, FirstName = e.FirstName, HireDate = e.HireDate, IsActive = e.IsActive, LastName = e.LastName, Position = e.Position });
        if (employee == null)
            return new NotFoundResult();
        
        return Ok(employee);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployee(EmployeeDTO employee, CredentialDTO credential)
    {
        var newEmployee = new Employee
        {
            FirstName = employee.FirstName,
            HireDate = employee.HireDate,
            IsActive = employee.IsActive,
            LastName = employee.LastName,
            Position = employee.Position,
        };
        await _context.Employees.AddAsync(newEmployee);
        await _context.SaveChangesAsync();

        var newCredential = new Credential
        {
            PasswordHash = credential.PasswordHash,
            Role = credential.Role,
            Username = credential.Username,
            EmployeeId = newEmployee.Id
        };
        await _context.Credentials.AddAsync(newCredential);
        await _context.SaveChangesAsync();
        
        return Ok();
    }
}