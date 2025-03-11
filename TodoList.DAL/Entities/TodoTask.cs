using System.ComponentModel.DataAnnotations;

namespace TodoList.DAL.Entities;

public class TodoTask
{
    [Key]
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public TaskStatus? Status { get; set; }
}

public enum TaskStatus
{
    Pending,
    InProgress,
    Completed
}