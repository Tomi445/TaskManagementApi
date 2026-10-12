
using System.Net;
using TaskManagementApi.Entities;
using TaskManagementApi.Repositories;

namespace TaskManagementApi.Services;

public class TaskNotificationService : ITaskNotificationService
{
    private readonly IUserRepository _users;
    private readonly IEmailService _email;
    private readonly ILogger<TaskNotificationService> _logger;

    public TaskNotificationService(
        IUserRepository users,
        IEmailService email,
        ILogger<TaskNotificationService> logger)
    {
        _users = users;
        _email = email;
        _logger = logger;
    }

    public async Task NotifyTaskAssignedAsync(TaskItem task)
    {
        if (task.AssignedToUserId is null)
        {
            return;
        }

        try
        {
            var user = await _users.GetByIdAsync(task.AssignedToUserId.Value);
            if (user is null)
            {
                return;
            }

            var subject = $"New task assigned: {task.Title}";
            var body = $"""
                <h2>You have been assigned a task</h2>
                <p><strong>Title:</strong> {WebUtility.HtmlEncode(task.Title)}</p>
                <p><strong>Description:</strong> {WebUtility.HtmlEncode(task.Description ?? "-")}</p>
                <p><strong>Status:</strong> {task.Status}</p>
                <p><strong>Due:</strong> {task.DueDate:dddd, dd MMM yyyy HH:mm}</p>
                """;

            await _email.SendAsync(user.Email, subject, body);
            _logger.LogInformation("Assignment email sent to {Email} for task {TaskId}", user.Email, task.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send assignment email for task {TaskId}", task.Id);
        }
    }
}