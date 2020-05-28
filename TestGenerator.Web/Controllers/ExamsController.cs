using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestGenerator.Model.Data;
using TestGenerator.Model.Entities;
using TestGenerator.Web.Models;

namespace TestGenerator.Web.Controllers
{
    [Authorize]
    public class ExamsController : Controller
    {
        private readonly TestGeneratorContext _context;

        public ExamsController(TestGeneratorContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Exam> examList = null;

            if (this.User.IsInRole("Administrator"))
            {
                examList = _context.Exams
                    .Include(e => e.Questions)
                    .ThenInclude(e => e.Question)
                    .ToList();
            }
            else
            {
                examList = _context.Exams
                    .Where(e => e.ClosingDate > DateTime.Now)
                    .ToList();
            }

            return View(examList);
        }

        [HttpGet]
        [Authorize(Roles="Administrator")]
        public IActionResult Create()
        {
            return View(new ExamCreationViewModel
            {
                Questions = _context.Questions.ToList(),
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Administrator")]
        public async Task<IActionResult> Create(ExamCreationViewModel viewModel)
        {
            if (!ModelState.IsValid || _context.Questions.Count() < viewModel.QuestionAmount)
            {
                return BadRequest(ModelState);
            }

            var selectedQuestions = RetrieveQuestions(viewModel.QuestionAmount);

            var exam = new Exam
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                QuestionAmount = viewModel.QuestionAmount,
                AuthorizedAttempts = viewModel.AuthorizedAttempts,
                Duration = viewModel.Duration,
                ClosingDate = viewModel.ClosingDate,
            };

            await _context.Exams.AddAsync(exam);
            await _context.ExamQuestions.AddRangeAsync(GenerateQuestionsToExamFromQuestionListAndExam(exam, selectedQuestions));

            _context.SaveChanges();

            return RedirectToAction("Index", "Exams");
        }

        [HttpGet]
        [Authorize(Roles="Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _context.Exams
                .Include(e => e.Questions)
                .ThenInclude(e => e.Question)
                .FirstOrDefaultAsync(e => e.ExamId == id);

            if (exam == null)
            {
                return NotFound();
            }

            return View(exam);
        }

        public virtual List<Question> RetrieveQuestions(int limit)
        {
            if (limit < 1)
            {
                throw new ArgumentException("Limit parameter must be greater than 0");
            }

            return _context.Questions
                .OrderBy(r => Guid.NewGuid())
                .Take(limit)
                .ToList();
        }

        public virtual ICollection<ExamQuestion> GenerateQuestionsToExamFromQuestionListAndExam(Exam exam, ICollection<Question> questions)
        {
            var examQuestions = new List<ExamQuestion>();

            foreach (var question in questions)
            {
                examQuestions.Add(new ExamQuestion { Exam = exam, Question = question });
            }

            return examQuestions;
        }
    }
}
