using System;
using System.Collections.Generic;
using TestGenerator.Model.Entities;

namespace TestGenerator.Web.Models
{
    public class ExamViewModel
    {
        public int ExamId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int QuestionAmount { get; set; }

        public int AuthorizedAttempts { get; set; }

        public int Duration { get; set; }

        public DateTime ClosingDate { get; set; }

        public Module Module { get; set; }

        public ICollection<ExamQuestion> Questions { get; set; }

        public ICollection<Model.Entities.ExamAttempt> UserExamAttempts { get; set; }
    }
}