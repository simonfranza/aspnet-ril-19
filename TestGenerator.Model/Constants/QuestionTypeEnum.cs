using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestGenerator.Model.Constants
{
    public enum QuestionTypeEnum
    {
        [Display(Name = "Question Binaire - Oui / Non")]
        YesNo,
        [Display(Name = "QCU - Question à Choix Unique")]
        SingleChoice,
        [Display(Name = "QCM - Question à Choix Multiples")]
        MultipleChoice,
        [Display(Name = "Code à Trous")]
        Code,
        [Display(Name = "Question Ouverte")]
        Open
    }
}
