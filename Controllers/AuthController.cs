using Microsoft.AspNetCore.Mvc;
using ToDoListAPIJWT.Data;
using ToDoListAPIJWT.Models;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly JwtService _jwtService;

    public AuthController(ApplicationDbContext context, JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] User user)
    {
        if (_context.Users.Any(u => u.Username == user.Username))
        {
            return BadRequest("User already exists.");
        }

        _context.Users.Add(user);
        _context.SaveChanges();
        return Ok("User registered.");
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] User user)
    {
        var dbUser = _context.Users.SingleOrDefault(u => u.Username == user.Username && u.Password == user.Password);
        if (dbUser == null)
        {
            return Unauthorized("Invalid credentials.");
        }

        var tokenResponse = _jwtService.GenerateToken(user.Username);

        return Ok(new
        {
            token = tokenResponse.Token,
            expiration = tokenResponse.Expiration.ToString("o") // Format expiration as ISO 8601 string
        });
    }
}
