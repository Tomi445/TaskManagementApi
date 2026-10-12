
using TaskManagementApi.Entities;

namespace TaskManagementApi.Services;

public interface ITokenService
{
    (string Token, DateTime ExpiresAt) CreateToken(User user);
}