using Microsoft.EntityFrameworkCore;
using TodoList.DAL.Data;
using TodoList.DAL.Entities;

namespace TodoList.DAL.Repositories;

public class TodoRepository(TodoDbContext dbContext) : IRepository<TodoTask>
{
    public async Task<IEnumerable<TodoTask>> GetAll()
    {
        return await dbContext.TodoTasks.ToListAsync();
    }

    public async Task<TodoTask> GetById(int id)
    {
        return await dbContext.TodoTasks.FindAsync(id);
    }

    public async Task Add(TodoTask entity)
    {
        await dbContext.AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task Update(TodoTask entity)
    {
        dbContext.Update(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(TodoTask entity)
    {
        dbContext.Remove(entity);
        await dbContext.SaveChangesAsync();
    }
}