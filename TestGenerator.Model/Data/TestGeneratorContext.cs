using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestGenerator.Model.Entities;

namespace TestGenerator.Model.Data
{
    public class TestGeneratorContext : IdentityDbContext<User>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<ExamAttempt> ExamAttempts { get; set; }



        public TestGeneratorContext(DbContextOptions<TestGeneratorContext> options)
            : base(options)
        {
        }
    }
}
