using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestGenerator.Model.Entities
{
    public class Answer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnswerId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Text { get; set; }

        public bool IsValid { get; set; }

        [Required]
        [Column("QuestionId")]
        public int QuestionId { get; set; }
    }
}
