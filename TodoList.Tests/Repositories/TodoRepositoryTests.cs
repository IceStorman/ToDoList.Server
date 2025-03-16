using Microsoft.EntityFrameworkCore;
using TodoList.DAL.Data;
using TodoList.DAL.Entities;
using TodoList.DAL.Repositories;

namespace TodoList.Tests.Repositories;

public class TodoRepositoryTests
{
    private readonly List<TodoTask> _testDbData = 
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

    [Fact]
    private async Task GetAllTasks_ShouldReturnAllTasks()
    {
        var context = await GetInMemoryDbContext();
        var repository = new TodoRepository(context);
        
        var tasks = await repository.GetAll();
        Assert.Equal(_testDbData, tasks);
    }

    [Fact]
    private async Task GetTaskById_ShouldReturnTaskById()
    {
        var context = await GetInMemoryDbContext();
        var repository = new TodoRepository(context);
        
        var tasks = await repository.GetAll();
        foreach (var item in tasks)
        {
            var foundTask = await repository.GetById(item.Id);
            Assert.NotNull(foundTask);
        }
    }

    [Fact]
    private async Task GetTaskById_ShouldReturnNull()
    {
        var context = await GetInMemoryDbContext();
        var repository = new TodoRepository(context);
        
        var task = await repository.GetById(-1);
        Assert.Null(task);
    }

    [Fact]
    private async Task AddTask_ShouldAddTask()
    {
        var context = await GetInMemoryDbContext();
        var repository = new TodoRepository(context);

        TodoTask testTask = new() {Title = "Test task", Description = "Test task description", Status = TodoTaskStatus.Pending, DueDate = DateTime.Today};
        await repository.Add(testTask);

        var expectedList = new List<TodoTask>(_testDbData) { testTask };
        
        Assert.Equal(expectedList, await repository.GetAll());
    }

    [Fact]
    private async Task UpdateTask_ShouldUpdateTask()
    {
        var context = await GetInMemoryDbContext();
        var repository = new TodoRepository(context);
        const string testTitle = "Updated task";
        
        var taskToUpdate = await repository.GetById(1);
        taskToUpdate.Title = testTitle;
        await repository.Update(taskToUpdate);
        
        var updatedTask = await repository.GetById(1);
        
        Assert.Equal(testTitle, updatedTask.Title);
    }

    [Fact]
    private async Task DeleteTask_ShouldDeleteTask()
    {
        var context = await GetInMemoryDbContext();
        var repository = new TodoRepository(context);
        
        var taskToDelete = await repository.GetById(1);
        await repository.Delete(taskToDelete);
        
        List<TodoTask> expectedList = new(_testDbData);
        expectedList.Remove(taskToDelete);
        
        Assert.Equal(expectedList, await repository.GetAll());
    }

    [Fact]
    private async Task DeleteAllTasks_ShouldDeleteAllTasks()
    {
        var context = await GetInMemoryDbContext();
        var repository = new TodoRepository(context);
        
        await repository.DeleteAll();
        
        Assert.Equal([], await repository.GetAll());
    }
    
    private async Task<TodoDbContext> GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var dbContext = new TodoDbContext(options);

        Seeder seeder = new(dbContext);
        await seeder.SeedData(_testDbData);

        return dbContext;
    }
}