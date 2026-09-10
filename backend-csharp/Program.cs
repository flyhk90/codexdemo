using Microsoft.EntityFrameworkCore;
using backend_csharp.Models;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("hello");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var databaseOptions = builder.Configuration
    .GetSection(DatabaseOptions.SectionName)
    .Get<DatabaseOptions>() ?? new DatabaseOptions();

builder.Services.Configure<DatabaseOptions>(
    builder.Configuration.GetSection(DatabaseOptions.SectionName));

if (!string.Equals(databaseOptions.Provider, "Sqlite", StringComparison.OrdinalIgnoreCase))
{
    throw new InvalidOperationException($"Unsupported database provider: {databaseOptions.Provider}");
}

var connectionString = builder.Configuration.GetConnectionString(databaseOptions.ConnectionStringName);
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        $"Connection string '{databaseOptions.ConnectionStringName}' is not configured.");
}

builder.Services.AddDbContext<ScoreContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy("WeChatPolicy", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    if (databaseOptions.EnsureCreated)
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ScoreContext>();
        dbContext.Database.EnsureCreated();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("WeChatPolicy");
app.MapControllers();

app.Run();
