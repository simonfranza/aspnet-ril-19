using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestGenerator.Model.Entities
{
    public class ExamParticipation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExamParticipationId { get; set; }

        public int Result { get; set; }

        [Required]
        [Column("ExamId")]
        public int ExamId { get; set; }

        [Required]
        [Column("PersonId")]
        public int PersonId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ParticipationDate { get; set; }
    }
}
