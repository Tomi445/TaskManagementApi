
using TaskManagementApi.DTOs;

namespace TaskManagementApi.Services;

public interface IAuthService
{
    Task<AuthResponseDto?> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto?> LoginAsync(LoginDto dto);
}