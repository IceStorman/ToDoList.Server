using TodoList.BLL.DTO;
using TodoList.DAL.Entities;
using TodoList.DAL.Repositories;

namespace TodoList.BLL.Services;

public class TodoService(IRepository<TodoTask> todoRepository)
{
    public async Task<IEnumerable<TodoTask>> GetAllTasks()
    {
        return await todoRepository.GetAll();
    }

    public async Task<TodoTask?> GetTaskById(int id)
    {
        return await todoRepository.GetById(id);
    }

    public async Task<TodoTask?> CreateTask(string title)
    {
        if (string.IsNullOrEmpty(title))
        {
            return null;
        }
        
        TodoTask task = new() { Title = title };
        await todoRepository.Add(task);
        
        return task;
    }

    public async Task<TodoTask?> UpdateTask(int id, TodoTaskDto taskDto)
    {
        var taskToUpdate = await GetTaskById(id);

        if (taskToUpdate is null)
        {
            return null;
        }
        
        taskToUpdate.Title = taskDto.Title;
        taskToUpdate.Description = taskDto.Description;
        taskToUpdate.Status = taskDto.Status;
        
        return taskToUpdate;
    }

    public async Task DeleteTask(int id)
    {
        var taskToDelete = await GetTaskById(id);

        if (taskToDelete is null)
        {
            return;
        }
        await todoRepository.Delete(taskToDelete);
    }
}