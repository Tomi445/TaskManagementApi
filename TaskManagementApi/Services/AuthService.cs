
using Microsoft.AspNetCore.Identity;
using TaskManagementApi.DTOs;
using TaskManagementApi.Entities;
using TaskManagementApi.Repositories;

namespace TaskManagementApi.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _users;
    private readonly ITokenService _tokens;
    private readonly PasswordHasher<User> _hasher = new();

    public AuthService(IUserRepository users, ITokenService tokens)
    {
        _users = users;
        _tokens = tokens;
    }

    public async Task<AuthResponseDto?> RegisterAsync(RegisterDto dto)
    {
        var email = dto.Email.Trim().ToLowerInvariant();

        if (await _users.EmailExistsAsync(email))
        {
            return null;
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            Role = UserRoles.User,
            CreatedAt = DateTime.UtcNow
        };
        user.PasswordHash = _hasher.HashPassword(user, dto.Password);

        await _users.CreateAsync(user);
        return BuildResponse(user);
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        var email = dto.Email.Trim().ToLowerInvariant();
        var user = await _users.GetByEmailAsync(email);

        if (user is null)
        {
            return null;
        }

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return BuildResponse(user);
    }

    private AuthResponseDto BuildResponse(User user)
    {
        var (token, expiresAt) = _tokens.CreateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            ExpiresAt = expiresAt,
            Email = user.Email,
            Role = user.Role
        };
    }
}