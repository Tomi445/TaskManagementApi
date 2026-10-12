
using TaskManagementApi.Entities;

namespace TaskManagementApi.Repositories;

public interface ITaskRepository
{
    Task<List<TaskItem>> GetAllAsync(TaskItemStatus? status);
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task<TaskItem> CreateAsync(TaskItem task);
    Task<TaskItem?> UpdateAsync(Guid id, TaskItem updatedTask);
    Task<bool> DeleteAsync(Guid id);
}