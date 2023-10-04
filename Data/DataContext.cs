using Microsoft.EntityFrameworkCore;
using TicketingSystem.Models;

namespace TicketingSystem.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        var adminUser = new User
        {
            ID = "0000000FFFFF",
            Email = "admin@example.com",
            Firstname = "Admin",
            Lastname = "User",
            RegisterDate = DateTime.Now,
            UserRole = "Administrator",
            Password = "123456789"
        };
        
        modelBuilder.Entity<User>().HasData(adminUser);
    }

}
