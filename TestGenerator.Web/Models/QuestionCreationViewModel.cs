﻿using System;
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

        public ICollection<Answer> Answers { get; set; }
    }
}