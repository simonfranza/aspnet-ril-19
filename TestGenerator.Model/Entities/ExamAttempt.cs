using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestGenerator.Model.Entities
{
    public class ExamAttempt
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExamAttemptId { get; set; }

        public int Result { get; set; }

        [Required]
        [Column("ExamId")]
        public int ExamId { get; set; }
        public Exam Exam { get; set; }

        [Required]
        [Column("UserId")]
        public string UserId { get; set; }
        public User User { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ParticipationDate { get; set; }
    }
}
