
using TaskManagementApi.Entities;

namespace TaskManagementApi.Repositories;

public interface ITaskRepository
{
    Task<(List<TaskItem> Items, int TotalCount)> GetAllAsync(TaskItemStatus? status, Guid? assignedToUserId, int page, int pageSize);
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task<TaskItem> CreateAsync(TaskItem task);
    Task<TaskItem?> UpdateAsync(Guid id, TaskItem updatedTask);
    Task<bool> DeleteAsync(Guid id);
}