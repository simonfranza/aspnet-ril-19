using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestGenerator.Model.Entities;

namespace TestGenerator.Web.Models
{
    public class ExamIndexViewModel
    {
        public ICollection<Exam> Exams { get; set; }

        public ICollection<Module> Modules { get; set; } = new List<Module>();

        public string ModuleId = "";
    }
}
