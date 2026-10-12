
using TaskManagementApi.Entities;

namespace TaskManagementApi.Services;

public interface ITaskNotificationService
{
    Task NotifyTaskAssignedAsync(TaskItem task);
}