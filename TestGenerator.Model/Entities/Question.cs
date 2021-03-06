﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestGenerator.Model.Constants;

namespace TestGenerator.Model.Entities
{
    public class Question
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionId { get; set; }

        [Required]
        [MaxLength(255)]
        [DataType(DataType.Text)]
        public string Text { get; set; }

        [Required]
        public QuestionTypeEnum QuestionType { get; set; }

        [Display(Name = "Examens")]
        public ICollection<ExamQuestion> Exams { get; set; }

        [Display(Name = "Réponses")]
        public ICollection<Answer> Answers { get; set; }

        [Required]
        [Column("ModuleId")]
        public int ModuleId { get; set; }
        public Module Module { get; set; }
    }
}
