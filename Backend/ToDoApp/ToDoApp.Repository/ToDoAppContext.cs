using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Repository.Models;

namespace ToDoApp.Repository;

public partial class ToDoAppContext : DbContext
{
    public ToDoAppContext()
    {
    }

    public ToDoAppContext(DbContextOptions<ToDoAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<TaskInfo> TaskInfos { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserTask> UserTasks { get; set; }

/*    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.0.0.27;Database=BikramToDoApp;Trusted_Connection=True;encrypt=False");*/

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Status__3214EC07170C10AE");

            entity.ToTable("Status");

            entity.Property(e => e.StatusName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Status_Name");
        });

        modelBuilder.Entity<TaskInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TaskInfo__3214EC07656AE9A5");

            entity.ToTable("TaskInfo");

            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07A608EC06");

            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserTask__3214EC07E66BF108");

            entity.ToTable("UserTask");

            entity.Property(e => e.CompletedOn)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Flag).HasDefaultValue(false);

            entity.HasOne(d => d.Status).WithMany(p => p.UserTasks)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK__UserTask__Status__571DF1D5");

            entity.HasOne(d => d.Task).WithMany(p => p.UserTasks)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserTask__TaskId__5629CD9C");

            entity.HasOne(d => d.User).WithMany(p => p.UserTasks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserTask__Flag__5535A963");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
