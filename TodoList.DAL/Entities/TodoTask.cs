using System.ComponentModel.DataAnnotations;

namespace TodoList.DAL.Entities;

public class TodoTask
{
    [Key]
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public TodoTaskStatus Status { get; set; }
}

public enum TodoTaskStatus
{
    Pending,
    InProgress,
    Completed,
    Invalid
}