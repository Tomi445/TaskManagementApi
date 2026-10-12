
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.DTOs;
using TaskManagementApi.Services;

namespace TaskManagementApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth)
    {
        _auth = auth;
    }

    // POST /api/auth/register
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto)
    {
        var result = await _auth.RegisterAsync(dto);
        if (result is null)
        {
            return Conflict(new { message = "Email is already registered." });
        }

        return StatusCode(StatusCodes.Status201Created, result);
    }

    // POST /api/auth/login
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
    {
        var result = await _auth.LoginAsync(dto);
        if (result is null)
        {
            return Unauthorized(new { message = "Invalid email or password." });
        }

        return Ok(result);
    }
}