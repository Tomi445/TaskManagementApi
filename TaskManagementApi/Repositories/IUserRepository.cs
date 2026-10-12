
using TaskManagementApi.Entities;

namespace TaskManagementApi.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
    Task<User> CreateAsync(User user);
    Task<bool> ExistsAsync(Guid id);
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(Guid id);
}