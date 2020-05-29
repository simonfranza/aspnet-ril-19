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
        [Display(Name = "Titre")]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        public string Description { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "Nombre de questions")]
        public int QuestionAmount { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "Nombre de tentatives")]
        public int AuthorizedAttempts { get; set; }

        [Required]
        [Display(Name = "Durée de l'examen")]
        public int Duration { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ClosingDate { get; set; }

        [Required]
        [Column("ModuleId")]
        public int ModuleId { get; set; }
        public Module Module { get; set; }

        public ICollection<ExamQuestion> Questions { get; set; }
    }
}
