using Microsoft.EntityFrameworkCore;
using TodoList.DAL.Data;
using TodoList.DAL.Entities;
using TodoList.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IRepository<>));
builder.Services.AddScoped<TodoRepository>();

var app = builder.Build();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
        await context.Database.MigrateAsync();
    
        Seeder seeder = new(context);
        await seeder.SeedData();
    }
    catch (Exception ex)
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
        throw;
    }
}

app.Run();