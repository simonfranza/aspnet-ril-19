using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            return View(_context.Questions.ToList());
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
                QuestionType = questionViewModel.QuestionType,
                Text = questionViewModel.Text
            };

            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new QuestionCreationViewModel { Answers = new List<Answer> { new Answer() } });
        }

        [HttpGet]
        public IActionResult AddMultiAnswer()
        {
            var viewModel = new QuestionCreationViewModel { Answers = new List<Answer>() };
            viewModel.Answers.Add(new Answer());

            return View(viewModel);
        }
    }
}