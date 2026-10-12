
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.DTOs;
using TaskManagementApi.Entities;
using TaskManagementApi.Repositories;

namespace TaskManagementApi.Controllers;

[ApiController]
[Route("api/users")]
[Authorize(Roles = UserRoles.Admin)]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _users;

    public UsersController(IUserRepository users)
    {
        _users = users;
    }

    // GET /api/users  (Admin only)
    [HttpGet]
    public async Task<ActionResult<List<UserResponseDto>>> GetAll()
    {
        var users = await _users.GetAllAsync();
        return Ok(users.Select(u => u.ToResponseDto()).ToList());
    }
}