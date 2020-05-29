using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestGenerator.Model.Entities;
using TestGenerator.Model.Helpers;

namespace TestGenerator.Web.Models
{
    public class ExamCreationViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [MaxLength(255)]
        [Display(Name = "Intitulé")]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        public string Description { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "Nombre de questions")]
        public int QuestionAmount { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "Nombre d'essais par participant")]
        public int AuthorizedAttempts { get; set; }

        [Required]
        [Display(Name = "Durée (en minutes)")]
        public int Duration { get; set; }

        [Required]
        [Display(Name = "Date de cloturation")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [ExamDateValidator]
        public DateTime ClosingDate { get; set; }

        public ICollection<ExamQuestion> ExamQuestions { get; set; }

        public ICollection<Question> Questions { get; set; }

        [Required(ErrorMessage = "L'examen doit appartenir à un module.")]
        [Display(Name = "Module de cours")]
        public int ModuleId { get; set; }

        public List<SelectListItem> Modules { get; set; }
    }
}
