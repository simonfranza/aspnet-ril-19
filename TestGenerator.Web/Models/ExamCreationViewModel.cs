using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestGenerator.Web.Models
{
    public class ExamCreationViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [MaxLength(255)]
        [Display(Name = "Intitulé")]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        public string Description { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "Nombre de questions")]
        public int QuestionAmount { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "Nombre d'essais par participant")]
        public int AuthorizedAttempts { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Durée")]
        public int Duration { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Date de clôture")]
        public DateTime ClosingDate { get; set; }
    }
}
