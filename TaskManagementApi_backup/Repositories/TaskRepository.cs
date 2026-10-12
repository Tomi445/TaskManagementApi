
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

    public async Task<List<TaskItem>> GetAllAsync(TaskItemStatus? status)
    {
        var query = _context.Tasks.AsNoTracking().AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(t => t.Status == status.Value);
        }

        return await query.ToListAsync();
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