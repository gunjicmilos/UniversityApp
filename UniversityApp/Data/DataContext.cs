using Microsoft.EntityFrameworkCore;
using UniversityApp.Models;
using UniversityManagament.Models;

namespace UniversityApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<University> Universities { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserFaculty> UserFaculties { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<UserSubject> UserSubjects { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamPeriod> ExamPeriods { get; set; }
        public DbSet<UserExam> UserExams { get; set; }
        public DbSet<Finance> Finances { get; set; }
        public DbSet<BankTransaction> BankTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<University>()
                .HasMany(u => u.Faculties)
                .WithOne(u => u.University)
                .HasForeignKey(f => f.UniversityId);

            modelBuilder.Entity<UserFaculty>()
                .HasKey(uf => new { uf.UserId, uf.FacultyId });

            modelBuilder.Entity<UserFaculty>()
                .HasOne(uf => uf.User)
                .WithMany(u => u.UserFaculties)
                .HasForeignKey(uf => uf.UserId);

            modelBuilder.Entity<UserFaculty>()
                .HasOne(uf => uf.Faculty)
                .WithMany(f => f.UserFaculties)
                .HasForeignKey(uf => uf.FacultyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserSubject>()
                .HasKey(uf => new { uf.UserId, uf.SubjectId });

            modelBuilder.Entity<UserSubject>()
                .HasOne(uf => uf.User)
                .WithMany(u => u.UserSubjects)
                .HasForeignKey(uf => uf.UserId);

            modelBuilder.Entity<UserSubject>()
                .HasOne(uf => uf.Subject)
                .WithMany(f => f.UserSubjects)
                .HasForeignKey(uf => uf.SubjectId);


            modelBuilder.Entity<UserExam>()
                .HasKey(ue => new { ue.UserId, ue.ExamId });

            modelBuilder.Entity<UserExam>()
                .HasOne(ue => ue.User)
                .WithMany(u => u.UserExams)
                .HasForeignKey(ue => ue.UserId);

            modelBuilder.Entity<UserExam>()
                .HasOne(ue => ue.Exam)
                .WithMany(u => u.UserExams)
                .HasForeignKey(ue => ue.ExamId);

            modelBuilder.Entity<ExamPeriod>()
                .HasOne(ep => ep.Faculty)
                .WithMany(f => f.ExamPeriods)
                .HasForeignKey(ep => ep.FacultyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
