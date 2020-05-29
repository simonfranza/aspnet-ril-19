using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestGenerator.Web.Models
{
    public class AnswerCreationViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string Text { get; set; }

        public bool IsValid { get; set; }

        public int QuestionId { get; set; }

        public string Index { get; set; }
    }
}
