using ContactManager.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Contact> Contacts => Set<Contact>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Contact>().HasData(
            new Contact
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "9876543210",
                Company = "Google",
                Address = "New York",
                Favorite = true,
                CreatedAt = DateTime.UtcNow
            },
            new Contact
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@example.com",
                PhoneNumber = "9123456780",
                Company = "Microsoft",
                Address = "California",
                Favorite = false,
                CreatedAt = DateTime.UtcNow
            }
        );
    }
}