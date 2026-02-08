using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Auth;
using WebApplication1.CQRS.DTO;
using WebApplication1.DB;
using WebApplication1.DTO.AuthDTO;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : Controller
{
    private readonly Smeni1135Context _context;
    
    public AuthController(Smeni1135Context context)
    {
        _context = context;
    }
    
    [HttpPost]
    public async Task<ActionResult> Login(LoginData loginData)
    {
        var user = await _context.Credentials.FirstOrDefaultAsync(s=>s.Username ==  loginData.Login && s.PasswordHash == loginData.Password);
        if (user == null)
            return new NotFoundResult();
        
        var claims = new List<Claim> {
            new Claim(ClaimValueTypes.Integer32, user.Id.ToString())
        };
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                    
        string token = new JwtSecurityTokenHandler().WriteToken(jwt);

        return new OkObjectResult(token);
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<ProfileResponse>> GetProfile()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized();
        }

        var userId = int.Parse(userIdClaim);
        var credential = await _context.Credentials
            .Include(c => c.Employee)
            .FirstOrDefaultAsync(c => c.Id == userId);

        if (credential == null || credential.Employee == null)
        {
            return NotFound("Не нашли....");
        }

        return Ok(new ProfileResponse
        {
            Employee = new EmployeeProfile
            {
                Id = credential.Employee.Id,
                FirstName = credential.Employee.FirstName,
                LastName = credential.Employee.LastName,
                Position = credential.Employee.Position
            },
            Role = credential.Role
        });
    }
    
    
}