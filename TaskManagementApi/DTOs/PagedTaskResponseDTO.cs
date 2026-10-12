
namespace TaskManagementApi.DTOs;

public class PagedTaskResponseDto
{
    public List<TaskResponseDto> Items { get; set; } = new();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}