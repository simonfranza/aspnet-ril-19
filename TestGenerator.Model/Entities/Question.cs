using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestGenerator.Model.Entities
{
    public class Question
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionId { get; set; }

        public ICollection<Answer> Answers { get; set; }

        [Required]
        [Column("ModuleId")]
        public int ModuleId { get; set; }
        public Module Module { get; set; }
    }
}
