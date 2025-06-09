using Microsoft.EntityFrameworkCore;
using BIL551_LanguageLearningPlatform.Models;
using System.Reflection;

namespace BIL551_LanguageLearningPlatform.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Submission> Submissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Teacher)
                .WithMany()
                .HasForeignKey(c => c.TeacherID)
                .OnDelete(DeleteBehavior.Restrict); // <== disable cascade delete here

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Language)
                .WithMany(l => l.Courses)
                .HasForeignKey(c => c.LanguageID)
                .OnDelete(DeleteBehavior.Cascade); // this is fine

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.User)
                .WithMany(u => u.Enrollments)
                .HasForeignKey(e => e.UserID)
                .OnDelete(DeleteBehavior.Cascade); // keep one cascade


        }
    }
}
