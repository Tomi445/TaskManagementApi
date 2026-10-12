
using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Entities;

namespace TaskManagementApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<TaskItem> Tasks => Set<TaskItem>();
}