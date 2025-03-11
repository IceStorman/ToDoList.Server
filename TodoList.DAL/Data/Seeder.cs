using TodoList.DAL.Entities;
using TaskStatus = TodoList.DAL.Entities.TaskStatus;

namespace TodoList.DAL.Data;

public class Seeder
{
    private readonly ApplicationDbContext _dbContext;
    
    public Seeder(ApplicationDbContext dbContext)
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
                    Status = TaskStatus.Pending
                },
                new()
                {
                    Title = "Make coffee",
                    Description = "The best part of the day",
                    Status = TaskStatus.InProgress
                },
                new()
                {
                    Title = "Go work",
                    Description = "Just the part of the day",
                    Status = TaskStatus.Completed
                }
            ];
        
        _dbContext.AddRange(data);
        
        await _dbContext.SaveChangesAsync();
    }
}