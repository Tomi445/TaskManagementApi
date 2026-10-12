using TaskManagementApi.Repositories;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;

var builder = WebApplication.CreateBuilder(args);

// ===== Register services =====
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

var app = builder.Build();

// ===== Pipeline =====
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();