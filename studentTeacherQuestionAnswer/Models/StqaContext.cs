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

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Reply> Replies { get; set; }

    public virtual DbSet<UserInfo> UserInfos { get; set; }

/*    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=DESKTOP-0FBCPHT\\SQLEXPRESS; database=stqa; trusted_connection=true; TrustServerCertificate=True");
*/
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.Aid).HasName("PK__Answer__C69007C8CFAC7752");

            entity.ToTable("Answer");

            entity.Property(e => e.Aid).HasColumnName("AID");
            entity.Property(e => e.Aby).HasColumnName("ABy");
            entity.Property(e => e.Adate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("ADate");
            entity.Property(e => e.Answer1)
                .HasColumnType("text")
                .HasColumnName("Answer");

            entity.HasOne(d => d.AbyNavigation).WithMany(p => p.Answers)
                .HasForeignKey(d => d.Aby)
                .HasConstraintName("FK__Answer__ABy__4CA06362");

            entity.HasOne(d => d.AforNavigation).WithMany(p => p.Answers)
                .HasForeignKey(d => d.Afor)
                .HasConstraintName("FK__Answer__Afor__52593CB8");
        });

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

        modelBuilder.Entity<Reply>(entity =>
        {
            entity.HasKey(e => e.Rid).HasName("PK__Reply__CAFF413224FD1A7A");

            entity.ToTable("Reply");

            entity.Property(e => e.Rid).HasColumnName("RID");
            entity.Property(e => e.Rby).HasColumnName("RBy");
            entity.Property(e => e.Rdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("RDate");
            entity.Property(e => e.Reply1)
                .HasColumnType("text")
                .HasColumnName("Reply");
            entity.Property(e => e.Rfor).HasColumnName("RFor");

            entity.HasOne(d => d.RbyNavigation).WithMany(p => p.Replies)
                .HasForeignKey(d => d.Rby)
                .HasConstraintName("FK__Reply__RBy__5070F446");

            entity.HasOne(d => d.RforNavigation).WithMany(p => p.Replies)
                .HasForeignKey(d => d.Rfor)
                .HasConstraintName("FK__Reply__RFor__5165187F");
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
