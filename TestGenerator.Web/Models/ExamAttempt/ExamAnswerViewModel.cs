using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestGenerator.Web.Models.ExamAttempt
{
    public class ExamAnswerViewModel
    {
        public string Text { get; set; }

        public int AnswerId { get; set; }

        public bool StudentAnswer { get; set; }
    }
}
