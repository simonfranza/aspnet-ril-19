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
        public DbSet<ExamQuestion> ExamQuestions { get; set; }


        public TestGeneratorContext(DbContextOptions<TestGeneratorContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ExamQuestion>()
                .HasKey(examQuestion => new
                {
                    examQuestion.ExamId,
                    examQuestion.QuestionId
                });

            builder.Entity<ExamQuestion>()
                .HasOne(examQuestion => examQuestion.Exam)
                .WithMany(exam => exam.Questions)
                .HasForeignKey(examQuestion => examQuestion.ExamId);

            builder.Entity<ExamQuestion>()
                .HasOne(examQuestion => examQuestion.Question)
                .WithMany(question => question.Exams)
                .HasForeignKey(examQuestion => examQuestion.QuestionId);
        }
    }
}
