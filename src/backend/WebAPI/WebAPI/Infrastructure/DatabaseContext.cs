using WebAPI.Models.Database;
using Task = WebAPI.Models.Database.Task;

namespace WebAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

public class DatabaseContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<List> Lists { get; set; }
    public DbSet<Task> Tasks { get; set; }
    
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
            
    }
}