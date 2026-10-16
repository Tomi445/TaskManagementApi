
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using TaskManagementApi.DTOs;
using TaskManagementApi.Entities;
using TaskManagementApi.Repositories;
using TaskManagementApi.Services;

namespace TaskManagementApi.Controllers;

[ApiController]
[Route("api/tasks")]
[Authorize]   
public class TasksController : ControllerBase
{
    private readonly ITaskRepository _repository;
    private readonly IUserRepository _users;
    private readonly ITaskNotificationService _notifications;   // NEW

    public TasksController(ITaskRepository repository, IUserRepository users, ITaskNotificationService notifications)   // NEW parameter
    {
        _repository = repository;
        _users = users;
        _notifications = notifications;   // NEW
    }
    // GET /api/tasks?status=Pending&page=1&pageSize=10
    // Admin: all tasks. Customer: only tasks assigned to them.
    [HttpGet]
    public async Task<ActionResult<PagedTaskResponseDto>> GetAll(
        [FromQuery] TaskItemStatus? status,
        [FromQuery, Range(1, int.MaxValue)] int page = 1,
        [FromQuery, Range(1, 100)] int pageSize = 10)
    {
        Guid? assignedToUserId = null;

        if (!User.IsInRole(UserRoles.Admin))
        {
            assignedToUserId = GetCurrentUserId();
            if (assignedToUserId is null)
            {
                return Unauthorized();
            }
        }

        var (tasks, totalCount) = await _repository.GetAllAsync(status, assignedToUserId, page, pageSize);

        return Ok(new PagedTaskResponseDto
        {
            Items = tasks.Select(t => t.ToResponseDto()).ToList(),
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        });
    }

    // GET /api/tasks/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TaskResponseDto>> GetById(Guid id)
    {
        var task = await _repository.GetByIdAsync(id);
        if (task is null)
        {
            return NotFound();
        }

        // Customers can only see their own tasks. Others look "not found".
        if (!User.IsInRole(UserRoles.Admin) && task.AssignedToUserId != GetCurrentUserId())
        {
            return NotFound();
        }

        return Ok(task.ToResponseDto());
    }

    // POST /api/tasks  (Admin only)
    [HttpPost]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<ActionResult<TaskResponseDto>> Create(CreateTaskDto dto)
    {
        if (!await _users.ExistsAsync(dto.AssignedToUserId!.Value))
        {
            return BadRequest(new { message = "The assigned user does not exist." });
        }

        var created = await _repository.CreateAsync(dto.ToEntity());
        await _notifications.NotifyTaskAssignedAsync(created);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created.ToResponseDto());
    }

    // PUT /api/tasks/{id}  (Admin only)
    [HttpPut("{id:guid}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<ActionResult<TaskResponseDto>> Update(Guid id, UpdateTaskDto dto)
    {
        if (!await _users.ExistsAsync(dto.AssignedToUserId!.Value))
        {
            return BadRequest(new { message = "The assigned user does not exist." });
        }

        var existing = await _repository.GetByIdAsync(id);
        if (existing is null)
        {
            return NotFound();
        }

        var previousAssignee = existing.AssignedToUserId;

        var updated = await _repository.UpdateAsync(id, dto.ToEntity());
        if (updated is null)
        {
            return NotFound();
        }

        if (updated.AssignedToUserId != previousAssignee)
        {
            await _notifications.NotifyTaskAssignedAsync(updated);
        }

        return Ok(updated.ToResponseDto());
    }

    // DELETE /api/tasks/{id}  (Admin only)
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _repository.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    // Reads the logged-in user's id from their token
    private Guid? GetCurrentUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? User.FindFirstValue("sub");

        return Guid.TryParse(value, out var id) ? id : null;
    }
}