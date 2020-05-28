using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestGenerator.Web.Models
{
    public class ModuleCreationViewModel
    {
        [Required]
        [MaxLength(255)]
        [Display(Name="Nom du module")]
        public string Title { get; set; }

        [Display(Name="Description du module")]
        public string Description { get; set; }
    }
}
