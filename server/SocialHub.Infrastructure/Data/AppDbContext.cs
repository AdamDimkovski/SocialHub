namespace SocialHub.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using SocialHub.Core.Entities;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Import citext type
            modelBuilder.HasPostgresExtension("citext");

            modelBuilder.Entity<User>(entity =>
            {   
                // Table name / Primary Key
                entity.ToTable("users", tableBuilder =>
                {
                    tableBuilder.HasCheckConstraint("CK_username_length", "LENGTH(username) BETWEEN 3 AND 50");
                    tableBuilder.HasCheckConstraint("CK_email_length", "LENGTH(email) BETWEEN 3 AND 100");
                }
                );

                entity.HasKey(u => u.Id);

                // Id
                entity.Property(u => u.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid")
                    .HasDefaultValueSql("gen_random_uuid()");

                // Username
                entity.Property(u => u.Username)
                    .HasColumnName("username")
                    .IsRequired()
                    .HasColumnType("citext");

                entity.HasIndex(u => u.Username)
                    .IsUnique();

                // Email
                entity.Property(u => u.Email)
                    .HasColumnName("email")
                    .IsRequired()
                    .HasColumnType("citext");

                entity.HasIndex(u => u.Email)
                    .IsUnique();

                // Password
                entity.Property(u => u.PasswordHash)
                    .HasColumnName("password_hash")
                    .IsRequired()
                    .HasColumnType("text");

                // CreatedAt
                entity.Property(u => u.CreatedAt)
                    .HasColumnName("created_at")
                    .IsRequired()
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("now()")
                    .HasColumnType("timestamptz");

                // One-to-Many relationship: fill out later.
            });
                
        }
    
}