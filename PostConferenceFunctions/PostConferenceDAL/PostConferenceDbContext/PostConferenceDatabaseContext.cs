using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace PostConferenceDAL.PostConferenceDbContext
{
    public partial class PostConferenceDatabaseContext : DbContext
    {
        public PostConferenceDatabaseContext()
        {
        }

        public PostConferenceDatabaseContext(DbContextOptions<PostConferenceDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Attendee> Attendees { get; set; }
        public virtual DbSet<Speaker> Speakers { get; set; }
        public virtual DbSet<Webinar> Webinars { get; set; }
        public virtual DbSet<WebinarFeedback> WebinarFeedbacks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Attendee>(entity =>
            {
                entity.ToTable("Attendee");

                entity.Property(e => e.AttendeeId).HasColumnName("AttendeeID");

                entity.Property(e => e.WebinarId).HasColumnName("WebinarID");

                entity.Property(e => e.DiplomaUrl).IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Speaker>(entity =>
            {
                entity.ToTable("Speaker");

                entity.Property(e => e.SpeakerId).HasColumnName("SpeakerID");

                entity.Property(e => e.WebinarId).HasColumnName("WebinarID");

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Webinar>(entity =>
            {
                entity.ToTable("Webinar");

                entity.Property(e => e.WebinarId).HasColumnName("WebinarID");

                entity.Property(e => e.EndDateTime).HasColumnType("datetime");

                entity.Property(e => e.WebinarName).IsUnicode(false);

                entity.Property(e => e.FeedbackFormUrl).IsUnicode(false);

                entity.Property(e => e.DiplomaTemplateUrl).IsUnicode(false);

                entity.Property(e => e.OnlineMeetingJoinUrl).IsUnicode(false);

                entity.Property(e => e.StartDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<WebinarFeedback>(entity =>
            {
                entity.ToTable("WebinarFeedback");

                entity.Property(e => e.WebinarFeedbackId).HasColumnName("WebinarFeedbackID");

                entity.Property(e => e.Comments).IsUnicode(false);

                entity.Property(e => e.FeedbackDateTime).HasColumnType("datetime");

                entity.Property(e => e.NextTopics).IsUnicode(false);

                entity.Property(e => e.WebinarId).HasColumnName("WebinarID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
