using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestGenerator.Model.Entities
{
    public class UserAnswer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserAnswerId { get; set; }

        public bool IsValid { get; set; }

        [Required]
        [Column("QuestionId")]
        public int QuestionId { get; set; }

        [Required]
        [Column("AnswserId")]
        public int AnswerId { get; set; }

        [Required]
        [Column("ExamAttemptId")]
        public int ExamAttemptId { get; set; }
    }
}
