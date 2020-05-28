using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

            await _context.AddAsync(moduleData);
            await _context.SaveChangesAsync();

            return View(moduleViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
    }
}