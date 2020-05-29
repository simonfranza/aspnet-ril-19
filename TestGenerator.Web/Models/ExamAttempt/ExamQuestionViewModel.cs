using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TestGenerator.Model.Constants;
using TestGenerator.Model.Entities;
using TestGenerator.Web.Models.ExamAttempt;

namespace TestGenerator.Web.Models
{
    public class ExamQuestionViewModel
    {
        public int QuestionId { get; set; }

        public string Text { get; set; }

        public QuestionTypeEnum QuestionType { get; set; }

        public List<ExamAnswerViewModel> Answers { get; set; }
    }
}
