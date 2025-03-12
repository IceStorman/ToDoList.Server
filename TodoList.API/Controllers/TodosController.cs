using Microsoft.AspNetCore.Mvc;
using TodoList.BLL.DTO;
using TodoList.BLL.Services;

namespace TodoList.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TodosController(TodoService todoService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await todoService.GetAllTasks());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var task = await todoService.GetTaskById(id);

        if (task is null)
        {
            return NotFound();
        }
        return Ok(task);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Create([FromBody]TodoTaskDto taskDto)
    {
        var task = await todoService.CreateTask(taskDto.Title);
        
        if (task is null)
        {
            return BadRequest();
        }
        return Ok(task);
    }

    [HttpPut("[action]/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TodoTaskDto taskDto)
    {
        var updatedTask = await todoService.UpdateTask(id, taskDto, false);

        if (updatedTask is null)
        {
            return BadRequest();
        }
        return Ok(updatedTask);
    }

    [HttpPut("[action]/{id}")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] TodoTaskDto taskDto)
    {
        var updatedTask = await todoService.UpdateTask(id, taskDto, true);

        if (updatedTask is null)
        {
            return BadRequest();
        }
        return Ok(updatedTask);
    }

    [HttpDelete("[action]/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await todoService.DeleteTask(id);
        return Ok();
    }

    [HttpDelete("[action]")]
    public async Task<IActionResult> DeleteAll()
    {
        await todoService.DeleteAllTasks();
        return Ok();
    }
}