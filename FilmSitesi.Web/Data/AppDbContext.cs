using FilmSitesi.Web.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FilmSitesi.Web.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<WatchlistItem> WatchlistItems => Set<WatchlistItem>();
    public DbSet<WatchedMovie> WatchedMovies => Set<WatchedMovie>();
    public DbSet<Activity> Activities => Set<Activity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Movie>()
            .HasIndex(m => m.TmdbId)
            .IsUnique();

        modelBuilder.Entity<Movie>()
            .Property(m => m.Title)
            .HasMaxLength(200);

        modelBuilder.Entity<Movie>()
            .Property(m => m.OriginalTitle)
            .HasMaxLength(200);

        modelBuilder.Entity<Movie>()
            .Property(m => m.OriginalLanguage)
            .HasMaxLength(20);

        modelBuilder.Entity<Review>()
            .Property(r => r.Rating)
            .HasDefaultValue(0);

        modelBuilder.Entity<Review>()
            .Property(r => r.Comment)
            .HasMaxLength(2000);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.Movie)
            .WithMany(m => m.Reviews)
            .HasForeignKey(r => r.MovieId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Review>()
            .HasIndex(r => new { r.UserId, r.MovieId })
            .IsUnique();

        modelBuilder.Entity<WatchlistItem>()
            .Property(w => w.Priority)
            .HasDefaultValue(3);

        modelBuilder.Entity<WatchlistItem>()
            .Property(w => w.Notes)
            .HasMaxLength(1000);

        modelBuilder.Entity<WatchlistItem>()
            .HasOne(w => w.User)
            .WithMany(u => u.WatchlistItems)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<WatchlistItem>()
            .HasOne(w => w.Movie)
            .WithMany(m => m.WatchlistItems)
            .HasForeignKey(w => w.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<WatchlistItem>()
            .HasIndex(w => new { w.UserId, w.MovieId })
            .IsUnique();

        modelBuilder.Entity<WatchedMovie>()
            .Property(w => w.Notes)
            .HasMaxLength(1000); 

        modelBuilder.Entity<WatchedMovie>()
            .HasOne(w => w.User)
            .WithMany(u => u.WatchedMovies)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<WatchedMovie>()
            .HasOne(w => w.Movie)
            .WithMany(m => m.WatchedMovies)
            .HasForeignKey(w => w.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Activity>()
            .HasOne(a => a.User)
            .WithMany(u => u.Activities)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Activity>()
            .HasOne(a => a.Movie)
            .WithMany(m => m.Activities)
            .HasForeignKey(a => a.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Activity>()
            .Property(a => a.Type)
            .HasMaxLength(50);

        modelBuilder.Entity<Activity>()
            .Property(a => a.Note)
            .HasMaxLength(1000);

    }
}