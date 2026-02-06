using Microsoft.AspNetCore.Mvc;
using WebApplication1.DB;

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
}