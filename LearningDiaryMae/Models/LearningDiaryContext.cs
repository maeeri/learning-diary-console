

#nullable disable

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
namespace LearningDiaryMae.Models
{
    public partial class LearningDiaryContext : DbContext
    {
        public LearningDiaryContext()
        {
        }

        public LearningDiaryContext(DbContextOptions<LearningDiaryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<DiaryTask> Tasks { get; set; }
        public virtual DbSet<DiaryTopic> Topics { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost\\;Database=LearningDiary;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Note>(entity =>
            {
                entity.Property(e => e.Note1)
                    .HasMaxLength(140)
                    .IsUnicode(false);

                entity.Property(e => e.Note2)
                    .HasMaxLength(140)
                    .IsUnicode(false);

                entity.Property(e => e.Note3)
                    .HasMaxLength(140)
                    .IsUnicode(false);

                entity.Property(e => e.Note4)
                    .HasMaxLength(140)
                    .IsUnicode(false);

                entity.Property(e => e.Note5)
                    .HasMaxLength(140)
                    .IsUnicode(false);

                entity.HasOne(d => d.TaskNavigation)
                    .WithMany(p => p.NotesNavigation)
                    .HasForeignKey(d => d.Task)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("task_id");
            });

            modelBuilder.Entity<DiaryTask>(entity =>
            {
                entity.ToTable("task");

                entity.Property(e => e.Deadline).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.TopicNavigation)
                    .WithMany(p => (IEnumerable<DiaryTask>)p.Tasks)
                    .HasForeignKey(d => d.Topic)
                    .HasConstraintName("FK__task__topic_id__36B12243");
            });

            modelBuilder.Entity<DiaryTopic>(entity =>
            {
                entity.ToTable("topic");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastEditDate).HasColumnType("datetime");

                entity.Property(e => e.Source)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.StartLearningDate).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
