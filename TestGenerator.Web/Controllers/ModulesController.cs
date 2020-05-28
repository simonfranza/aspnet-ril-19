using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestGenerator.Model.Data;
using TestGenerator.Model.Entities;
using TestGenerator.Web.Models;

namespace TestGenerator.Web.Controllers
{
    public class ModulesController : Controller
    {
        private readonly TestGeneratorContext _context;

        public ModulesController(TestGeneratorContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Module> moduleList = _context.Modules
                .Include(m => m.Exams)
                .ToList();

            return View(moduleList);
        }

        [HttpPost]
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
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var module = await _context.Modules
                .FirstOrDefaultAsync(m => m.ModuleId == id);
            if (module == null)
            {
                return NotFound();
            }

            return View(module);
        }
    }
}