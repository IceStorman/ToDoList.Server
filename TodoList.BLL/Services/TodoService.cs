using Microsoft.IdentityModel.Tokens;
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
        if (title.TrimStart().IsNullOrEmpty())
        {
            return null;
        }
        
        TodoTask task = new() { Title = title };
        await todoRepository.Add(task);
        
        return task;
    }

    public async Task<TodoTask?> UpdateTask(int id, TodoTaskDto taskDto, bool shouldModifyOnlyStatus)
    {
        var taskToUpdate = await GetTaskById(id);

        if (taskToUpdate is null || taskDto.Title.TrimStart().IsNullOrEmpty() && !shouldModifyOnlyStatus)
        {
            return null;
        }

        if(!shouldModifyOnlyStatus)
        {
            taskToUpdate.Title = taskDto.Title;
            taskToUpdate.Description = taskDto.Description;
            taskToUpdate.DueDate = taskDto.DueDate;
        }
        
        taskToUpdate.Status = (int)taskDto.Status >= 0 && (int)taskDto.Status < (int)TodoTaskStatus.Invalid ?
            taskDto.Status : TodoTaskStatus.Pending;

        await todoRepository.Update(taskToUpdate);
        
        return taskToUpdate;
    }

    public async Task<TodoTask?> UpdateDueDate(int id, TodoTaskDto taskDto)
    {
        var taskToUpdate = await GetTaskById(id);

        if (taskToUpdate is null)
        {
            return null;
        }
        
        taskToUpdate.DueDate = taskDto.DueDate;

        await todoRepository.Update(taskToUpdate);
        
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

    public async Task DeleteAllTasks()
    {
        await todoRepository.DeleteAll();
    }
}