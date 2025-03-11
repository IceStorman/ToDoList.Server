using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TodoList.DAL.Data;

namespace TodoList.DAL.Factories;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<TodoDbContext>
{
    public TodoDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TodoDbContext>();
        
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=TodoList;User Id=Vlad;Password=password;TrustServerCertificate=True");

        return new TodoDbContext(optionsBuilder.Options);
    }
}