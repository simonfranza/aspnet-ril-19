using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles="Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Create(ExamCreationViewModel viewModel)
        {
            if (!ModelState.IsValid || _context.Questions.Count() < viewModel.QuestionAmount)
            {
                return BadRequest(ModelState);
            }

            var selectedQuestions = RetrieveQuestions(viewModel.QuestionAmount);

            await _context.Exams.AddAsync(new Exam()
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                QuestionAmount = viewModel.QuestionAmount,
                AuthorizedAttempts = viewModel.AuthorizedAttempts,
                Duration = viewModel.Duration,
                ClosingDate = viewModel.ClosingDate,
                Questions = selectedQuestions,
            });

            return View(viewModel);
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
    }
}
