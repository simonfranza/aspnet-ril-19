using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestGenerator.Model.Data;
using TestGenerator.Model.Entities;
using TestGenerator.Web.Models;

namespace TestGenerator.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly TestGeneratorContext _context;
        private readonly UserManager<User> _userManager;

        public UserController(TestGeneratorContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegistrationViewModel userViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = userViewModel.Email,
                Password = userViewModel.Password,
                Firstname = userViewModel.FirstName,
                Lastname = userViewModel.LastName,
            };

            await _context.Users.AddAsync(user, default(CancellationToken));

            _context.SaveChanges();

            if (user.Email.Contains("@cesi.fr"))
            {
                await _userManager.AddToRoleAsync(user, "Administrator");
            }
            
            return View(userViewModel);
        }
    }
}
