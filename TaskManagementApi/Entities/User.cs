
using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Entities;

public class User
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Role { get; set; } = UserRoles.User;

    public DateTime CreatedAt { get; set; }
}