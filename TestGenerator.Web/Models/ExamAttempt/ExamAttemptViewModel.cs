using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TestGenerator.Model.Entities;

namespace TestGenerator.Web.Models.ExamAttempt
{
    public class ExamAttemptViewModel
    {
        public int ExamId { get; set; }
        public Exam Exam { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public DateTime ParticipationDate { get; set; }

        public List<ExamQuestionViewModel> Questions { get; set; }
    }
}
