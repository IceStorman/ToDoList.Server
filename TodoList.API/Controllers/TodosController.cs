using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoList.DAL.Data;
using TodoList.DAL.Entities;

namespace TodoList.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TodosController(TodoDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<TodoTask>>> GetTodos()
    {
        return await dbContext.TodoTasks.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoTask>> GetTodo(int id)
    {
        TodoTask? task = await dbContext.TodoTasks.FindAsync(id);
        
        if (task == null)
        {
            return NotFound();
        }
        return task;
    }
}