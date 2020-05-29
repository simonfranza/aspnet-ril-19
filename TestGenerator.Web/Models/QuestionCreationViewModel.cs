using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TestGenerator.Model.Constants;
using TestGenerator.Model.Entities;

namespace TestGenerator.Web.Models
{
    public class QuestionCreationViewModel
    {
        [Required(ErrorMessage="La question doit avoir un texte.")]
        [MaxLength(255)]
        [DataType(DataType.Text)]
        [Display(Name ="Texte de la question")]
        public string Text { get; set; }

        [Required(ErrorMessage ="La question doit avoir un type.")]
        [Display(Name = "Type de question")]
        public QuestionTypeEnum QuestionType { get; set; }

        public List<AnswerCreationViewModel> Answers { get; set; }

        public List<AnswerCreationViewModel> BinaryAnswers { get; set; } = new List<AnswerCreationViewModel> { new AnswerCreationViewModel { Text = "Oui" }, new AnswerCreationViewModel { Text = "Non" } };

        [Required(ErrorMessage ="La question doit appartenir à un module.")]
        [Display(Name ="Module de cours")]
        public int ModuleId { get; set; }

        public List<SelectListItem> Modules { get; set; }
    }
}
