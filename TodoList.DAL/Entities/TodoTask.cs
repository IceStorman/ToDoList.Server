namespace TodoList.DAL.Entities;

public class TodoTask
{
    public int Id;
    public string? Title;
    public string? Description;
    public TaskStatus? Status;
}

public enum TaskStatus
{
    Pending,
    InProgress,
    Completed
}