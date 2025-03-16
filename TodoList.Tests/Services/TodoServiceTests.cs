using Microsoft.EntityFrameworkCore;
using TodoList.BLL.DTO;
using TodoList.BLL.Services;
using TodoList.DAL.Data;
using TodoList.DAL.Entities;
using TodoList.DAL.Repositories;

namespace TodoList.Tests.Services;

public class TodoServiceTests
{
    private TodoService _todoService;
    private TodoRepository _todoRepository;
    
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

    [Theory]
    [InlineData("Test title")]
    [InlineData("qwert      ")]
    [InlineData("123456")]
    [InlineData("     ffew")]
    private async Task CreateTask_ShouldCreateNewTask(string testTitle)
    {
        await UpdateDbContext();

        var actualTask = await _todoService.CreateTask(testTitle);
        
        Assert.NotNull(actualTask);
        Assert.Equal(testTitle, actualTask.Title);
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    private async Task CreateTask_ShouldReturnNull(string testTitle)
    {
        await UpdateDbContext();
        
        var actualTask = await _todoService.CreateTask(testTitle);
        
        Assert.Null(actualTask);
    }
    
    [Fact]
    public async Task UpdateTask_ShouldReturnNull_WhenTaskNotFound()
    {
        await UpdateDbContext();
        
        const int nonExistentTaskId = -1;
        var taskDto = new TodoTaskDto { Title = "Updated Task", Status = TodoTaskStatus.Completed };
        
        var result = await _todoService.UpdateTask(nonExistentTaskId, taskDto, false);
        
        Assert.Null(result);
    }
    
    [Fact]
    public async Task UpdateTask_ShouldReturnNull_WhenInvalidTitleAndNotModifyOnlyStatus()
    {
        await UpdateDbContext();
        
        var existingTask = new TodoTask { Title = "Existing Task", Status = TodoTaskStatus.Pending };
        await _todoRepository.Add(existingTask);

        var taskDto = new TodoTaskDto { Title = "", Status = TodoTaskStatus.Completed };

        var result = await _todoService.UpdateTask(existingTask.Id, taskDto, false);
        
        Assert.Null(result);
    }
    
    [Fact]
    public async Task UpdateTask_ShouldUpdateTask_WhenValidDataAndModify()
    {
        await UpdateDbContext();
        
        var existingTask = new TodoTask { Title = "Existing Task", Status = TodoTaskStatus.Pending };
        await _todoRepository.Add(existingTask);

        var taskDto = new TodoTaskDto
        {
            Title = "Updated Task",
            Description = "Updated description",
            DueDate = DateTime.Now.AddDays(1),
            Status = TodoTaskStatus.Completed
        };
        
        var result = await _todoService.UpdateTask(existingTask.Id, taskDto, false);
        
        Assert.NotNull(result);
        Assert.Equal(taskDto.Title, result.Title);
        Assert.Equal(taskDto.Description, result.Description);
        Assert.Equal(taskDto.Status, result.Status);
    }
    
    [Fact]
    public async Task UpdateTask_ShouldOnlyUpdateStatus_WhenShouldModifyOnlyStatusIsTrue()
    {
        await UpdateDbContext();
        
        var existingTask = new TodoTask { Title = "Existing Task", Status = TodoTaskStatus.Pending };
        await _todoRepository.Add(existingTask);

        var taskDto = new TodoTaskDto
        {
            Title = "",
            Status = TodoTaskStatus.Completed
        };
        
        var result = await _todoService.UpdateTask(existingTask.Id, taskDto, true);
        
        Assert.NotNull(result);
        Assert.Equal(existingTask.Title, result.Title);
        Assert.Equal(taskDto.Status, result.Status);
    }

    [Fact]
    private async Task UpdateDueDate_ShouldReturnNull_WhenTaskNotFound()
    {
        await UpdateDbContext();
        
        const int nonExistentTaskId = -1;
        var taskDto = new TodoTaskDto {Title = "", Status = TodoTaskStatus.Completed};

        var actualTask = await _todoService.UpdateDueDate(nonExistentTaskId, taskDto);
        
        Assert.Null(actualTask);
    }

    [Fact]
    private async Task UpdateDueDate_ShouldReturnUpdatedTask()
    {
        await UpdateDbContext();

        var existingTask = _todoService.GetTaskById(1);
        var taskDto = new TodoTaskDto {Title = "", DueDate = DateTime.MaxValue};
        
        var actualTask = await _todoService.UpdateDueDate(existingTask.Id, taskDto);
        
        Assert.Equal(taskDto.DueDate, actualTask.DueDate);
    }

    [Fact]
    private async Task DeleteTask_ShouldNotDeleteTask_WhenTaskNotFound()
    {
        await UpdateDbContext();
        
        await _todoService.DeleteTask(-1);
        
        Assert.Equal(_testDbData, await _todoService.GetAllTasks());
    }
    
    [Fact]
    private async Task DeleteTask_ShouldDeleteTask()
    {
        await UpdateDbContext();

        await _todoService.DeleteTask(1);
        
        Assert.Null(await _todoService.GetTaskById(1));
    }

    private async Task UpdateDbContext()
    {
        var context = await GetInMemoryDbContext();
        _todoRepository = new TodoRepository(context);
        _todoService = new TodoService(_todoRepository);
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