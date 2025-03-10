using Microsoft.EntityFrameworkCore;
using TodoList.DAL.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Server=localhost;Database=TodoList;TrustServerCertificate=True")));

var app = builder.Build();

app.MapControllers();

app.Run();