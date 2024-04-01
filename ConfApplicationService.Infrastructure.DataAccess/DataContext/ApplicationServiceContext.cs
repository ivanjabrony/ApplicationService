using ConfApplicationService.Models.Applications;
using ConfApplicationService.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace ConfApplicationService.Implementation.DataContext;

public class ApplicationServiceContext : DbContext
{
    public ApplicationServiceContext(DbContextOptions<ApplicationServiceContext> options) 
        : base(options)
    {}
    
    public ApplicationServiceContext() 
        : base()
    {}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(@"Server=localhost;Port=5432;Database=Application-database;User ID=postgres;Password=postgres");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(u => u.Name).HasMaxLength(25);
        
        modelBuilder.Entity<FinishedApplication>()
            .Property(u => u.Description).HasMaxLength(300).IsRequired();
        modelBuilder.Entity<FinishedApplication>()
            .Property(u => u.Plan).HasMaxLength(1000).IsRequired();
        modelBuilder.Entity<FinishedApplication>()
            .Property(u => u.Title).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<FinishedApplication>()
            .Property(u => u.Activity).IsRequired();
        modelBuilder.Entity<FinishedApplication>()
            .Property(u => u.AuthorId).IsRequired();
        
        modelBuilder.Entity<DraftApplication>()
            .Property(u => u.Description).HasMaxLength(300);
        modelBuilder.Entity<DraftApplication>()
            .Property(u => u.Plan).HasMaxLength(1000);
        modelBuilder.Entity<DraftApplication>()
            .Property(u => u.Title).HasMaxLength(100);
        modelBuilder.Entity<DraftApplication>()
            .Property(u => u.AuthorId).IsRequired();
    }

    public DbSet<User> Users { get; set; }
    
    public DbSet<FinishedApplication> FinishedApplications { get; set; }
    public DbSet<DraftApplication> DraftApplications { get; set; }
}