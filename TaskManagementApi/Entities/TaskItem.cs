using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Entities;

public class TaskItem
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public TaskItemStatus Status { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? AssignedToUserId { get; set; }

    public User? AssignedToUser { get; set; }

}

