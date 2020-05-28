using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TestGenerator.Model.Entities
{
    public class Module
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ModuleId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        public string Description { get; set; }

        public ICollection<Question> Questions { get; set; }

        public ICollection<Exam> Exams { get; set; }
    }
}
