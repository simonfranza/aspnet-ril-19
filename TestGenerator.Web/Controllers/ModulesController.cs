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
    public class ModulesController : Controller
    {
        private readonly TestGeneratorContext _context;

        public ModulesController(TestGeneratorContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Module> moduleList = _context.Modules
                .Include(m => m.Exams)
                .ToList();

            return View(moduleList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ModuleCreationViewModel moduleViewModel)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var module= new Module
            {
                Title = moduleViewModel.Title,
                Description = moduleViewModel.Description
            };

            await _context.Modules.AddAsync(module);
            await _context.SaveChangesAsync();

            return RedirectToRoute(new { controller = "Modules", action = "Details", id = module.ModuleId });
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var module = await _context.Modules
                .Include(m => m.Exams)
                .Include(m => m.Questions)
                .FirstOrDefaultAsync(m => m.ModuleId == id);
            if (module == null)
            {
                return NotFound();
            }

            return View(module);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var module = await _context.Modules
                .FirstOrDefaultAsync(m => m.ModuleId == id);

            if(module == null)
            {
                return NotFound();
            }

            return View(module);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(Module moduleData)
        {
            if (moduleData == null)
            {
                return NotFound();
            }

            var module = _context.Update(moduleData);
            await _context.SaveChangesAsync();

            if (module == null)
            {
                return NotFound();
            }

            return View(module.Entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(Module moduleData)
        {
            if (moduleData == null)
            {
                return NotFound();
            }

            var module = await _context.Modules
                .Include(m => m.Questions)
                .Include(m => m.Exams)
                .ThenInclude(e => e.Questions)
                .FirstOrDefaultAsync(m => m.ModuleId == moduleData.ModuleId);

            if (module == null)
            {
                return NotFound();
            }

            foreach(Exam exam in module.Exams)
            {
                _context.ExamQuestions.RemoveRange(exam.Questions);
            }

            await _context.SaveChangesAsync();
            _context.Modules.Remove(module);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Modules");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var module = await _context.Modules
                .Include(m => m.Exams)
                .Include(m => m.Questions)
                .FirstOrDefaultAsync(m => m.ModuleId == id);

            if (module == null)
            {
                return NotFound();
            }

            return View(module);
        }
    }
}