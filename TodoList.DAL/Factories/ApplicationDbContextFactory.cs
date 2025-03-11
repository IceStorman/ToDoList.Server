using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TodoList.DAL.Data;

namespace TodoList.DAL.Factories;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=TodoList;User Id=Vlad;Password=password;TrustServerCertificate=True");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}