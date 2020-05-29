using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestGenerator.Model.Data;
using TestGenerator.Model.Entities;
using TestGenerator.Web.Models;
using TestGenerator.Web.Models.ExamAttempt;

namespace TestGenerator.Web.Controllers
{
    [Authorize]
    public class ExamsController : Controller
    {
        private readonly TestGeneratorContext _context;
        private readonly UserManager<User> _userManager;

        public ExamsController(TestGeneratorContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            List<Exam> examList = null;

            if (this.User.IsInRole("Administrator"))
            {
                examList = _context.Exams
                    .Include(e => e.Questions)
                    .ThenInclude(e => e.Question)
                    .Include(e => e.Module)
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
            var viewModel = new ExamCreationViewModel();
            viewModel.Questions = _context.Questions.ToList();
            viewModel.Modules = _context.Modules
                .Select(module => new SelectListItem {Text = module.Title, Value = "" + module.ModuleId})
                .ToList();

            return View(viewModel);
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
                ModuleId = viewModel.ModuleId,
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
                .Include(e => e.Module)
                .FirstOrDefaultAsync(e => e.ExamId == id);

            if (exam == null)
            {
                return NotFound();
            }

            return View(exam);
        }

        [HttpGet]
        public async Task<IActionResult> Attempt(int? id)
        {
            if(id == null)
            {
                NotFound();
            }

            var viewModel = new ExamAttemptViewModel
            {
                Exam = await _context.Exams.Include(e => e.Questions).ThenInclude(q => q.Question).ThenInclude(q => q.Answers).FirstOrDefaultAsync(e => e.ExamId == id),
                ExamId = (int)id,
                User = await _userManager.FindByIdAsync(_userManager.GetUserId(User)),
                UserId = _userManager.GetUserId(User),
                ParticipationDate = DateTime.Now
            };

            var questions = new List<ExamQuestionViewModel>();
            foreach(ExamQuestion question in viewModel.Exam.Questions)
            {
                var questionVM = new ExamQuestionViewModel
                {
                    QuestionId = question.Question.QuestionId,
                    Text = question.Question.Text,
                    QuestionType = question.Question.QuestionType
                };

                var answers = new List<ExamAnswerViewModel>();
                foreach(Answer answer in question.Question.Answers)
                {
                    var answerVM = new ExamAnswerViewModel
                    {
                        Text = answer.Text,
                        AnswerId = answer.AnswerId
                    };
                    answers.Add(answerVM);
                }

                questionVM.Answers = answers;

                questions.Add(questionVM);
            }

            viewModel.Questions = questions;

            if(viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Attempt(ExamAttemptViewModel viewModel)
        {
            if(viewModel == null)
            {
                return BadRequest();
            }

            viewModel.Exam = await _context.Exams.FirstOrDefaultAsync(e => e.ExamId == viewModel.ExamId);

            if(viewModel.ParticipationDate.AddMinutes(viewModel.Exam.Duration) > DateTime.Now)
            {
                return BadRequest();
            }

            var examAttempt = new ExamAttempt
            {
                ExamId = viewModel.ExamId,
                UserId = viewModel.UserId,
                ParticipationDate = viewModel.ParticipationDate
            };

            await _context.ExamAttempts.AddAsync(examAttempt);
            await _context.SaveChangesAsync();

            foreach(ExamQuestionViewModel question in viewModel.Questions)
            {
                foreach(ExamAnswerViewModel answer in question.Answers)
                {
                    if (answer.StudentAnswer)
                    {
                        var userAnswer = new UserAnswer 
                        { 
                            QuestionId = question.QuestionId, 
                            ExamAttemptId = examAttempt.ExamAttemptId,
                            AnswerId = answer.AnswerId
                        };
                        var realAnswer = await _context.Answers.FirstOrDefaultAsync(a => a.AnswerId == answer.AnswerId);
                        userAnswer.IsValid = realAnswer.IsValid;
                        await _context.UserAnswers.AddAsync(userAnswer);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            var userAnswers = _context.UserAnswers
                .Where(userAnswer => userAnswer.ExamAttemptId.Equals(examAttempt.ExamAttemptId))
                .ToList();

            examAttempt.Result = (userAnswers.Select(userAnswer => userAnswer.IsValid).Count() / userAnswers.Count()) * 100;

            var examAttemptData = _context.ExamAttempts.Update(examAttempt);

            if(examAttemptData == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index");
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
