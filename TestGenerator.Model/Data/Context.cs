using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestGenerator.Model.Entities;

namespace TestGenerator.Model.Data
{
    public class Context : IdentityDbContext<User>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<ExamParticipation> Participations { get; set; }



        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
