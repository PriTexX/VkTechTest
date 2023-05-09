using Microsoft.EntityFrameworkCore;
using VkTechTest.Database;
using VkTechTest.Models.Enums;

namespace VkTechTest.Tests;

/// <summary>
/// Добавляет общие функции для тестирования базы данных
/// </summary>
public abstract class DatabaseTestsHelper : IClassFixture<PostgresContainerManager>
{
    /// <summary>
    /// Создает инстанс ApplicationContext
    /// </summary>
    /// <returns><see cref="ApplicationContext"/></returns>
    protected static ApplicationContext CreateDbContext()
    {
        var builder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseNpgsql(PostgresContainerManager.ConnectionString);
        var dbContext = new ApplicationContext(builder.Options);

        return dbContext;
    }

    /// <summary>
    /// Очищает все записи в таблице users, кроме записи админа 
    /// </summary>
    /// <param name="ctx">Контекст базы данных</param>
    protected static async Task ClearDatabase(ApplicationContext ctx)
    {
        await ctx.Users
            .Where(u => u.UserGroup.Code != UserGroupType.Admin)
            .ExecuteDeleteAsync();
    }
}