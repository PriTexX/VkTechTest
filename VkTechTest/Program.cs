using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using VkTechTest;
using VkTechTest.DAO.Implementations;
using VkTechTest.DAO.Interfaces;
using VkTechTest.Database;
using VkTechTest.Services.Implementations;
using VkTechTest.Services.Interfaces;

LoadEnvVariablesFromFile("./.env");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(c => 
    c.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

builder.Services.AddSingleton<IPasswordHasher, SHA256PasswordHasher>();
builder.Services.AddScoped<IUserDAO, UserDao>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers()
    .AddJsonOptions(options => 
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo{Title = "User service", Version = "v1"});
    s.IncludeXmlComments("bin/Debug/VkTechTest.xml");
});

builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

builder.Services
    .AddOptions<ApplicationOptions>()
    .BindConfiguration(ApplicationOptions.SectionName)
    .ValidateOnStart()
    .ValidateDataAnnotations();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

void LoadEnvVariablesFromFile(string filePath)
{
    if (!File.Exists(filePath))
        return;

    foreach (var line in File.ReadAllLines(filePath))
    {
        var equalsIndex = line.IndexOf('=');

        if(equalsIndex == -1)
            continue;
        
        Environment.SetEnvironmentVariable(line[..equalsIndex], line[(equalsIndex + 1)..]);
    }
}