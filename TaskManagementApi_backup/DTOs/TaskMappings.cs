
using TaskManagementApi.Entities;

namespace TaskManagementApi.DTOs;

public static class TaskMappings
{
    public static TaskItem ToEntity(this CreateTaskDto dto) => new()
    {
        Title = dto.Title,
        Description = dto.Description,
        Status = dto.Status!.Value,
        DueDate = dto.DueDate!.Value
    };

    public static TaskItem ToEntity(this UpdateTaskDto dto) => new()
    {
        Title = dto.Title,
        Description = dto.Description,
        Status = dto.Status!.Value,
        DueDate = dto.DueDate!.Value
    };

    public static TaskResponseDto ToResponseDto(this TaskItem task) => new()
    {
        Id = task.Id,
        Title = task.Title,
        Description = task.Description,
        Status = task.Status,
        DueDate = task.DueDate,
        CreatedAt = task.CreatedAt,
        UpdatedAt = task.UpdatedAt
    };
}