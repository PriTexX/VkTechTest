using Microsoft.EntityFrameworkCore;
using VkTechTest.Database.Models;

namespace VkTechTest.Database;

public class ApplicationContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<UserGroupEntity> UserGroups { get; set; }
    public DbSet<UserStateEntity> UserStates { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    { }
}