using TodoList.DAL.Entities;

namespace TodoList.DAL.Data;

public class Seeder
{
    private readonly TodoDbContext _dbContext;
    
    public Seeder(TodoDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task SeedData()
    {
        if (_dbContext.TodoTasks.Any())
        {
            return;
        }

        List<TodoTask> data = 
            [
                new()
                {
                    Title = "Wake up",
                    Description = "The hardest part of the day",
                    Status = TodoTaskStatus.Pending,
                    DueDate = DateTime.Today
                },
                new()
                {
                    Title = "Make coffee",
                    Description = "The best part of the day",
                    Status = TodoTaskStatus.InProgress,
                    DueDate = DateTime.Today
                },
                new()
                {
                    Title = "Go work",
                    Description = "Just the part of the day",
                    Status = TodoTaskStatus.Completed,
                    DueDate = DateTime.Today
                }
            ];
        
        _dbContext.AddRange(data);
        
        await _dbContext.SaveChangesAsync();
    }
}