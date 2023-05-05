using Microsoft.EntityFrameworkCore;

namespace VkTechTest.Database;

public class ApplicationContext : DbContext
{
    
    
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    { }
}