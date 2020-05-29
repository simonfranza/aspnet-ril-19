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
            if (questionViewModel.Answers == null)
            {
                questionViewModel.Answers = questionViewModel.BinaryAnswers;
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var question = new Question
            {
                ModuleId = questionViewModel.ModuleId,
                QuestionType = questionViewModel.QuestionType,
                Text = questionViewModel.Text
            };

            await _context.Questions.AddAsync(question);

            var answersList = questionViewModel.Answers
                .Select(answerViewModel => new Answer()
                {
                    QuestionId = question.QuestionId,
                    Text = answerViewModel.Text,
                    IsValid = answerViewModel.IsValid
                })
                .ToList();

            await _context.Answers.AddRangeAsync(answersList);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Create()
        {
            var questionViewModel = new QuestionCreationViewModel();
            questionViewModel.Modules = _context.Modules
                .Select(module => new SelectListItem { Text = module.Title, Value = "" + module.ModuleId })
                .ToList();
            questionViewModel.Answers = new List<AnswerCreationViewModel> { new AnswerCreationViewModel() };

            return View(questionViewModel);
        }

        [HttpGet]
        public IActionResult AddBinaryAnswer()
        {
            return View(new QuestionCreationViewModel());
        }

        [HttpGet]
        public IActionResult AddMultiAnswer()
        {
            var viewModel = new QuestionCreationViewModel { Answers = new List<AnswerCreationViewModel>() };
            viewModel.Answers.Add(new AnswerCreationViewModel());

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult AddSingleAnswer()
        {
            var viewModel = new QuestionCreationViewModel { Answers = new List<AnswerCreationViewModel>() };
            viewModel.Answers.Add(new AnswerCreationViewModel());

            return View(viewModel);
        }
    }
}