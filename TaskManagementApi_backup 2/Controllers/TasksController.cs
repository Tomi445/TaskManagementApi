using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.DTOs;
using TaskManagementApi.Entities;
using TaskManagementApi.Repositories;

namespace TaskManagementApi.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly ITaskRepository _repository;

    public TasksController(ITaskRepository repository)
    {
        _repository = repository;
    }

    // GET /api/tasks?status=Pending&page=1&pageSize=10
    [HttpGet]
    public async Task<ActionResult<PagedTaskResponseDto>> GetAll(
        [FromQuery] TaskItemStatus? status,
        [FromQuery, Range(1, int.MaxValue)] int page = 1,
        [FromQuery, Range(1, 100)] int pageSize = 10)
    {
        var (tasks, totalCount) = await _repository.GetAllAsync(status, page, pageSize);

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

        return Ok(task.ToResponseDto());
    }

    // POST /api/tasks
    [HttpPost]
    public async Task<ActionResult<TaskResponseDto>> Create(CreateTaskDto dto)
    {
        var created = await _repository.CreateAsync(dto.ToEntity());
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created.ToResponseDto());
    }

    // PUT /api/tasks/{id}
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TaskResponseDto>> Update(Guid id, UpdateTaskDto dto)
    {
        var updated = await _repository.UpdateAsync(id, dto.ToEntity());
        if (updated is null)
        {
            return NotFound();
        }

        return Ok(updated.ToResponseDto());
    }

    // DELETE /api/tasks/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _repository.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}