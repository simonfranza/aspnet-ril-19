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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ModuleCreationViewModel moduleViewModel)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var moduleData = new Module
            {
                Title = moduleViewModel.Title,
                Description = moduleViewModel.Description
            };

            var module = await _context.Modules.AddAsync(moduleData);
            await _context.SaveChangesAsync();

            return RedirectToRoute(new { controller = "Modules", action = "Details", id = module.Entity.ModuleId });
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