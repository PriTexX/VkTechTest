using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using VkTechTest.Database;

namespace VkTechTest.Tests;

/// <summary>
/// Запускает контейнер с тестовой базой данных PostgreSQL, применяет миграции и создает строку подключения к бд
/// </summary>
public class DatabaseTests : IDisposable
{
    public static string ConnectionString;
    private static IContainer _pgContainer;

    public DatabaseTests()
    {
        const string postgresPwd = "pgpwd";

        _pgContainer = new ContainerBuilder()
            .WithName(Guid.NewGuid().ToString("N"))
            .WithImage("postgres:15")
            .WithHostname(Guid.NewGuid().ToString("N"))
            .WithExposedPort(5432)
            .WithPortBinding(5432, true)
            .WithEnvironment("POSTGRES_PASSWORD", postgresPwd)
            .WithEnvironment("PGDATA", "/pgdata")
            .WithTmpfsMount("/pgdata")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilCommandIsCompleted("psql -U postgres -c \"select 1\""))
            .Build();
        
        _pgContainer.StartAsync().GetAwaiter().GetResult();

        ConnectionString = new NpgsqlConnectionStringBuilder
        {
            Host = _pgContainer.Hostname,
            Port = _pgContainer.GetMappedPublicPort(5432),
            Password = postgresPwd,
            Database = "testDatabase",
            Username = "postgres"
        }.ConnectionString;

        var dbContext = CreateDbContext();
        
        dbContext.Database.Migrate();
        dbContext.Dispose();
    }

    /// <summary>
    /// Создает инстанс ApplicationContext
    /// </summary>
    /// <returns><see cref="ApplicationContext"/></returns>
    public static ApplicationContext CreateDbContext()
    {
        var builder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseNpgsql(ConnectionString);
        var dbContext = new ApplicationContext(builder.Options);

        return dbContext;
    }

    /// <summary>
    /// Очищает все записи в таблице users, кроме записи админа 
    /// </summary>
    /// <param name="ctx">Контекст базы данных</param>
    public static async Task ClearDatabase(ApplicationContext ctx)
    {
        await ctx.Users
            .Where(u => u.Login != "admin")
            .ExecuteDeleteAsync();
    }
    
    public void Dispose()
    {
        _pgContainer.DisposeAsync().GetAwaiter().GetResult();
    }
}