using Microsoft.EntityFrameworkCore;
using TodoList.DAL.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Server=localhost\\SQLEXPRESS;Database=TodoList;User Id=Vlad;Password=password;TrustServerCertificate=True")));

var app = builder.Build();

app.MapControllers();

app.Run();