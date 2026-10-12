
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Entities;

namespace TaskManagementApi.Data;

public static class DbSeeder
{
    public static async Task SeedAdminAsync(IServiceProvider services, IConfiguration config)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var email = config["AdminUser:Email"]!.Trim().ToLowerInvariant();

        if (await context.Users.AnyAsync(u => u.Email == email))
        {
            return;
        }

        var admin = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            Role = UserRoles.Admin,
            CreatedAt = DateTime.UtcNow
        };
        admin.PasswordHash = new PasswordHasher<User>().HashPassword(admin, config["AdminUser:Password"]!);

        context.Users.Add(admin);
        await context.SaveChangesAsync();
    }
}