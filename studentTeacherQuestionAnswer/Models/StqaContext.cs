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

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<UserInfo> UserInfos { get; set; }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Qid).HasName("PK__Question__CAB147CB7EC17AC2");

            entity.ToTable("Question");

            entity.Property(e => e.Qid).HasColumnName("QID");
            entity.Property(e => e.Qby).HasColumnName("QBy");
            entity.Property(e => e.Qdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("QDate");
            entity.Property(e => e.Question1)
                .HasColumnType("text")
                .HasColumnName("Question");

            entity.HasOne(d => d.QbyNavigation).WithMany(p => p.Questions)
                .HasForeignKey(d => d.Qby)
                .HasConstraintName("FK__Question__QBy__37A5467C");
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserInfo__1788CCAC1DF51856");

            entity.ToTable("UserInfo");

            entity.HasIndex(e => e.EmailId, "UQ__UserInfo__7ED91ACE172EFFAE").IsUnique();

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
