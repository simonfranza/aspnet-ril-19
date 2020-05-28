using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestGenerator.Model.Entities
{
    public class Exam
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExamId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MaxLength(255)]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        public string Description { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int QuestionAmount { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int AuthorizedAttempts { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ClosingDate { get; set; }

        public ICollection<ExamQuestion> Questions { get; set; }
    }
}
