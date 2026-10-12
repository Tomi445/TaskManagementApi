
using TaskManagementApi.Entities;

namespace TaskManagementApi.DTOs;

public static class UserMappings
{
    public static UserResponseDto ToResponseDto(this User user) => new()
    {
        Id = user.Id,
        Email = user.Email,
        Role = user.Role,
        CreatedAt = user.CreatedAt
    };
}