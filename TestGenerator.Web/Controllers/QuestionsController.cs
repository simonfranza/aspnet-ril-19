using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestGenerator.Model.Data;
using TestGenerator.Model.Entities;
using TestGenerator.Web.Models;

namespace TestGenerator.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class QuestionsController : Controller
    {
        private readonly TestGeneratorContext _context;

        public QuestionsController(TestGeneratorContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var questionList = _context.Questions
                .Include(e => e.Module)
                .Include(e => e.Exams)
                .ToList();

            return View(questionList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.Module)
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.QuestionId == id);

            if(question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Question questionData)
        {
            if (questionData == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.Answers)
                .Include(q => q.Module)
                .Include(q => q.Exams)
                .FirstOrDefaultAsync(q => q.QuestionId == questionData.QuestionId);

            if (question == null)
            {
                return NotFound();
            }

            if(question.Exams.Count != 0)
            {
                return Conflict();
            }

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Questions");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.Answers)
                .Include(q => q.Exams)
                .ThenInclude(q => q.Exam)
                .Include(q => q.Module)
                .FirstOrDefaultAsync(q => q.QuestionId == id);

            if (question == null)
            {
                return NotFound();
            }

            return View(question);
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