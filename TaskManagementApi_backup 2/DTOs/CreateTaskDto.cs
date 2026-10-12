
using System.ComponentModel.DataAnnotations;
using TaskManagementApi.Entities;

namespace TaskManagementApi.DTOs;

public class CreateTaskDto
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    [EnumDataType(typeof(TaskItemStatus))]
    public TaskItemStatus? Status { get; set; }

    [Required]
    public DateTime? DueDate { get; set; }
}