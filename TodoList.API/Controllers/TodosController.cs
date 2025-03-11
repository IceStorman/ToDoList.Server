using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TodoList.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TodosController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTodos()
    {
        
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTodo(int id)
    {
        
    }
}