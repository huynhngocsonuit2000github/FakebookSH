using Fakebook.Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fakebook.Auth.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.Id)
            .HasColumnName("id");

        builder.Property(user => user.Email)
            .HasColumnName("email")
            .HasMaxLength(320)
            .IsRequired();

        builder.Property(user => user.UserName)
            .HasColumnName("user_name")
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(user => user.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(user => user.LastName)
            .HasColumnName("last_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(user => user.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(user => user.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        builder.Property(user => user.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.HasIndex(user => user.Email)
            .IsUnique();

        builder.HasIndex(user => user.UserName)
            .IsUnique();
    }
}
