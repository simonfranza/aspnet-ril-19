using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestGenerator.Model.Data;
using TestGenerator.Model.Entities;
using TestGenerator.Web.Models;

namespace TestGenerator.Web.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly TestGeneratorContext _context;

        public QuestionsController(TestGeneratorContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var questionList = _context.Questions
                .Include(e => e.Module)
                .ToList();

            return View(questionList);
        }

        [HttpPost]
        public async Task<IActionResult> Create(QuestionCreationViewModel questionViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var question = new Question
            {
                ModuleId = questionViewModel.ModuleId,
                QuestionType = questionViewModel.QuestionType,
                Text = questionViewModel.Text,
                Answers = questionViewModel.Answers
            };

            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Create()
        {
            var questionViewModel = new QuestionCreationViewModel();
            questionViewModel.Modules = _context.Modules
                .Select(module => new SelectListItem {Text = module.Title, Value = "" + module.ModuleId})
                .ToList();
            
            return View(questionViewModel);
        }
    }
}