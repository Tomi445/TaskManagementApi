
using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.Entities;

namespace TaskManagementApi.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(List<TaskItem> Items, int TotalCount)> GetAllAsync(TaskItemStatus? status, int page, int pageSize)
    {
        var query = _context.Tasks.AsNoTracking().AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(t => t.Status == status.Value);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
    public async Task<TaskItem?> GetByIdAsync(Guid id)
    {
        return await _context.Tasks.FindAsync(id);
    }

    public async Task<TaskItem> CreateAsync(TaskItem task)
    {
        task.Id = Guid.NewGuid();
        task.CreatedAt = DateTime.UtcNow;

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return task;
    }

    public async Task<TaskItem?> UpdateAsync(Guid id, TaskItem updatedTask)
    {
        var existing = await _context.Tasks.FindAsync(id);
        if (existing is null)
        {
            return null;
        }

        existing.Title = updatedTask.Title;
        existing.Description = updatedTask.Description;
        existing.Status = updatedTask.Status;
        existing.DueDate = updatedTask.DueDate;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await _context.Tasks.FindAsync(id);
        if (existing is null)
        {
            return false;
        }

        _context.Tasks.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}