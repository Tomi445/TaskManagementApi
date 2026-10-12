
using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.Entities;

namespace TaskManagementApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<User> CreateAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Users.AnyAsync(u => u.Id == id);
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .OrderBy(u => u.Email)
            .ToListAsync();
    }
    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }
}