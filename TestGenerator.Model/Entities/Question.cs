using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestGenerator.Model.Entities
{
    public class Question
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionId { get; set; }

        [Required]
        public ICollection<Answer> Answers { get; set; }

        [Required]
        public ICollection<Answer> ValidAnswers { get; set; }
    }
}
