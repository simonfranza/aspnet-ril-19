using System.ComponentModel.DataAnnotations.Schema;

namespace TestGenerator.Model.Entities
{
    public class ExamQuestion
    {
        [Column("ExamId")]
        public int ExamId { get; set; }
        public Exam Exam { get; set; }

        [Column("QuestionId")]
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}