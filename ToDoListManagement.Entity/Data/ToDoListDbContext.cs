using Microsoft.EntityFrameworkCore;
using ToDoListManagement.Entity.Models;

namespace ToDoListManagement.Entity.Data;

public partial class ToDoListDbContext : DbContext
{
    public ToDoListDbContext(DbContextOptions<ToDoListDbContext> options)
        : base(options)
    {

    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectUser> ProjectUsers { get; set; }
    public DbSet<TaskAttachment> TaskAttachments { get; set; }
    public DbSet<ToDoList> ToDoLists { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ErrorLog> ErrorLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>().HasData(
            new User 
            { 
                UserId = 1,
                Name = "Admin",
                Email = "admin@outlook.com",
                Role = "Admin",
                PasswordHash = "$2a$11$YCiiJxUwumHUtegC05ahFej29UzVm/s1HRwPriPIuta4b.GWddWuW"
            }
        );
    }

}
