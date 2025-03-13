using TodoList.DAL.Entities;

namespace TodoList.BLL.DTO;

public class TodoTaskDto
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public TodoTaskStatus Status { get; set; }
    public DateTime DueDate { get; set; }
}