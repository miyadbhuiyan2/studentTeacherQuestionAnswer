using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace studentTeacherQuestionAnswer.Models;

public partial class StqaContext : DbContext
{
    public StqaContext()
    {
    }

    public StqaContext(DbContextOptions<StqaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<UserInfo> UserInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserInfo__1788CCACD930E32F");

            entity.ToTable("UserInfo");

            entity.HasIndex(e => e.EmailId, "UQ__UserInfo__7ED91ACE028FD76E").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.EmailId)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IdcardNo).HasColumnName("IDCardNo");
            entity.Property(e => e.InstituteName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserType)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
