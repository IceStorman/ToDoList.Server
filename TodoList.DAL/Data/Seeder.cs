using TodoList.DAL.Entities;

namespace TodoList.DAL.Data;

public class Seeder
{
    private readonly TodoDbContext _dbContext;
    
    public Seeder(TodoDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task SeedData(List<TodoTask> data)
    {
        if (_dbContext.TodoTasks.Any())
        {
            return;
        }
        
        _dbContext.AddRange(data);
        
        await _dbContext.SaveChangesAsync();
    }
}