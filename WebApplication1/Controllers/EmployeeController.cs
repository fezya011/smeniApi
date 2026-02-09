using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DB;
using WebApplication1.DTO;
using WebApplication1.DTO.EmployeeDTO;
using Employee = WebApplication1.DB.Employee;

namespace WebApplication1.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly Smeni1135Context _context;

        public EmployeesController(Smeni1135Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
        {
            var employees = await _context.Employees
                .Where(e => e.IsActive == true)
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Position = e.Position,
                    HireDate = e.HireDate ?? DateTime.MinValue,
                    IsActive = e.IsActive ?? false
                })
                .ToListAsync();

            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
        {
            var employee = await _context.Employees
                .Where(e => e.Id == id)
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Position = e.Position,
                    HireDate = e.HireDate ?? DateTime.MinValue,
                    IsActive = e.IsActive ?? false
                })
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<EmployeeDto>> CreateEmployee(CreateEmployeeDto createDto)
        {
            var existingUser = await _context.Credentials
                .FirstOrDefaultAsync(c => c.Username == createDto.Username);
            
            if (existingUser != null)
            {
                return BadRequest("Username already exists");
            }
            
            var employee = new Employee
            {
                FirstName = createDto.FirstName,
                LastName = createDto.LastName,
                Position = createDto.Position,
                HireDate = createDto.HireDate,
                IsActive = createDto.IsActive
            };

            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            
            var credential = new Credential
            {
                EmployeeId = employee.Id,
                Username = createDto.Username,
                PasswordHash = createDto.Password,
                Role = createDto.Role
            };

            await _context.Credentials.AddAsync(credential);
            await _context.SaveChangesAsync();

            var employeeDto = new EmployeeDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Position = employee.Position,
                HireDate = employee.HireDate ?? DateTime.MinValue,
                IsActive = employee.IsActive ?? false
            };

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employeeDto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateEmployee(int id, UpdateEmployeeDto updateDto)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            employee.FirstName = updateDto.FirstName;
            employee.LastName = updateDto.LastName;
            employee.Position = updateDto.Position;
            employee.HireDate = updateDto.HireDate;
            employee.IsActive = updateDto.IsActive;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
