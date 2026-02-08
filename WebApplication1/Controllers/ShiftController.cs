using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DB;
using WebApplication1.DTO.ShiftDTO;

namespace WebApplication1.Controllers;

[ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ShiftsController : ControllerBase
    {
        private readonly Smeni1135Context _context;

        public ShiftsController(Smeni1135Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShiftDto>>> GetShifts()
        {
            var shifts = await _context.Shifts
                .Include(s => s.Employee)
                .Select(s => new ShiftDto
                {
                    Id = s.Id,
                    EmployeeId = s.EmployeeId ?? 0,
                    StartDateTime = s.StartDateTime ?? DateTime.MinValue,
                    EndDateTime = s.EndDateTime ?? DateTime.MinValue,
                    Description = s.Description,
                    Employee = new EmployeeShortDto
                    {
                        Id = s.Employee.Id,
                        FirstName = s.Employee.FirstName,
                        LastName = s.Employee.LastName
                    }
                })
                .ToListAsync();

            return Ok(shifts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShiftDto>> GetShift(int id)
        {
            var shift = await _context.Shifts
                .Include(s => s.Employee)
                .Where(s => s.Id == id)
                .Select(s => new ShiftDto
                {
                    Id = s.Id,
                    EmployeeId = s.EmployeeId ?? 0,
                    StartDateTime = s.StartDateTime ?? DateTime.MinValue,
                    EndDateTime = s.EndDateTime ?? DateTime.MinValue,
                    Description = s.Description,
                    Employee = new EmployeeShortDto
                    {
                        Id = s.Employee.Id,
                        FirstName = s.Employee.FirstName,
                        LastName = s.Employee.LastName
                    }
                })
                .FirstOrDefaultAsync();

            if (shift == null)
            {
                return NotFound();
            }

            return Ok(shift);
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<IEnumerable<ShiftDto>>> GetShiftsByEmployee(int employeeId)
        {
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

            var shifts = await _context.Shifts
                .Include(s => s.Employee)
                .Where(s => s.EmployeeId == employeeId && 
                           s.StartDateTime >= thirtyDaysAgo)
                .Select(s => new ShiftDto
                {
                    Id = s.Id,
                    EmployeeId = s.EmployeeId ?? 0,
                    StartDateTime = s.StartDateTime ?? DateTime.MinValue,
                    EndDateTime = s.EndDateTime ?? DateTime.MinValue,
                    Description = s.Description,
                    Employee = new EmployeeShortDto
                    {
                        Id = s.Employee.Id,
                        FirstName = s.Employee.FirstName,
                        LastName = s.Employee.LastName
                    }
                })
                .ToListAsync();

            return Ok(shifts);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ShiftDto>> CreateShift(CreateShiftDto createDto)
        {
            if (createDto.EndDateTime <= createDto.StartDateTime)
            {
                return BadRequest("EndDateTime должен быть больше StartDateTime");
            }
            
            var employee = await _context.Employees.FindAsync(createDto.EmployeeId);
            if (employee == null)
            {
                return BadRequest("Не нашли...");
            }

            var shift = new Shift
            {
                EmployeeId = createDto.EmployeeId,
                StartDateTime = createDto.StartDateTime,
                EndDateTime = createDto.EndDateTime,
                Description = createDto.Description
            };

            await _context.Shifts.AddAsync(shift);
            await _context.SaveChangesAsync();

            var shiftDto = new ShiftDto
            {
                Id = shift.Id,
                EmployeeId = shift.EmployeeId ?? 0,
                StartDateTime = shift.StartDateTime ?? DateTime.MinValue,
                EndDateTime = shift.EndDateTime ?? DateTime.MinValue,
                Description = shift.Description,
                Employee = new EmployeeShortDto
                {
                    Id = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName
                }
            };

            return CreatedAtAction(nameof(GetShift), new { id = shift.Id }, shiftDto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateShift(int id, UpdateShiftDto updateDto)
        {
            if (updateDto.EndDateTime <= updateDto.StartDateTime)
            {
                return BadRequest("EndDateTime должен быть больше StartDateTime");
            }
            
            var employee = await _context.Employees.FindAsync(updateDto.EmployeeId);
            if (employee == null)
            {
                return BadRequest("Не нашли...");
            }

            var shift = await _context.Shifts.FindAsync(id);
            if (shift == null)
            {
                return NotFound();
            }

            shift.EmployeeId = updateDto.EmployeeId;
            shift.StartDateTime = updateDto.StartDateTime;
            shift.EndDateTime = updateDto.EndDateTime;
            shift.Description = updateDto.Description;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteShift(int id)
        {
            var shift = await _context.Shifts.FindAsync(id);
            if (shift == null)
            {
                return NotFound();
            }

            _context.Shifts.Remove(shift);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }