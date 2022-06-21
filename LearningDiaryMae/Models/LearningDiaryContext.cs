using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

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

        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<Topic> Topics { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=LearningDiary;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("task");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Deadline)
                    .HasColumnType("datetime")
                    .HasColumnName("deadline");

                entity.Property(e => e.Descr)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("descr");

                entity.Property(e => e.Done).HasColumnName("done");

                entity.Property(e => e.Notes)
                    .HasMaxLength(600)
                    .IsUnicode(false)
                    .HasColumnName("notes");

                entity.Property(e => e.Prty)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("prty");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.Property(e => e.TopicId).HasColumnName("topic_id");

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.TopicId)
                    .HasConstraintName("FK__task__topic_id__36B12243");
            });

            modelBuilder.Entity<Topic>(entity =>
            {
                entity.ToTable("topic");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descr)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("descr");

                entity.Property(e => e.InProgress).HasColumnName("in_progress");

                entity.Property(e => e.SourceOfStudy)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("source_of_study");

                entity.Property(e => e.StartLearningDate)
                    .HasColumnType("datetime")
                    .HasColumnName("start_learning_date");

                entity.Property(e => e.TimeSpent).HasColumnName("time_spent");

                entity.Property(e => e.TimeToMaster).HasColumnName("time_to_master");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("title");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
