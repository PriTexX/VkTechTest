using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using VkTechTest.Database;

LoadEnvVariablesFromFile(".env");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(c => 
    c.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo{Title = "User service", Version = "v1"});
    s.IncludeXmlComments("bin/Debug/VkTechTest.xml");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void LoadEnvVariablesFromFile(string filePath)
{
    if (!File.Exists(filePath))
        return;

    foreach (var line in File.ReadAllLines(filePath))
    {
        var parts = line.Split(
            '=',
            StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 2)
            continue;

        Environment.SetEnvironmentVariable(parts[0], parts[1]);
    }
}