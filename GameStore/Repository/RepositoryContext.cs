using System.Drawing;
using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repository.Configuration;

namespace Repository;

public class RepositoryContext : IdentityDbContext<User> 
{
    public RepositoryContext(DbContextOptions options)
        : base(options)
    {
    }   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // modelBuilder.Entity<Genre>().Property(t => t.Id).HasColumnName("GenreId");
        modelBuilder.Entity<Game>().Property(t => t.Id).HasColumnName("GameId");
      //  modelBuilder.Entity<Photo>().Property(t => t.Id).HasColumnName("PhotoId");
        
        // modelBuilder.Entity<Genre>().Property(g => g.Description).HasMaxLength(1024);
        // modelBuilder.Entity<Genre>().Property(g => g.Name).HasMaxLength(30).IsRequired();
        
        modelBuilder.Entity<Game>().Property(g => g.Title).HasMaxLength(500).IsRequired();
        modelBuilder.Entity<Game>().Property(g => g.Price).HasPrecision(2).IsRequired();
        modelBuilder.Entity<Game>().Property(g => g.Description).HasMaxLength(2000).IsRequired();
        
      //  modelBuilder.Entity<Photo>().Property(g => g.Description).HasMaxLength(500);
      //  modelBuilder.Entity<Photo>().Property(g => g.FileExtension).HasMaxLength(7);
        
        modelBuilder
            .Entity<User>()
            .HasMany<Game>(e => e.Games)
            .WithOne(e => e.User)
            .OnDelete(DeleteBehavior.Cascade);
        


        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        // modelBuilder.ApplyConfiguration(new UserConfiguration());
        // modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
        
        // modelBuilder.ApplyConfiguration(new GameConfiguration());
        
    }

    // public DbSet<Genre> Genres { get; set; }
    public DbSet<Game>? Games { get; set; }
    //public DbSet<Photo> Photos { get; set; }
}