using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using VkTechTest.Database;

namespace VkTechTest.Tests;

/// <summary>
/// Запускает контейнер с тестовой базой данных PostgreSQL, применяет миграции и создает строку подключения к бд
/// </summary>
public class PostgresContainerManager : IDisposable
{
    public static string ConnectionString;
    private static IContainer _pgContainer;

    public PostgresContainerManager()
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

        var builder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseNpgsql(ConnectionString);
        var dbContext = new ApplicationContext(builder.Options);
        
        dbContext.Database.Migrate();
        dbContext.Dispose();
    }

    public void Dispose()
    {
        _pgContainer.DisposeAsync().GetAwaiter().GetResult();
    }
}