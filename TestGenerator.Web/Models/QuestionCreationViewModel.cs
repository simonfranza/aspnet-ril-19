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
        public string Text { get; set; }

        [Required(ErrorMessage ="La question doit avoir un type.")]
        public QuestionTypeEnum QuestionType { get; set; }

        public ICollection<Answer> Answers { get; set; }
    }
}
