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

            var userId = Guid.NewGuid().ToString();

            var userData = new User
            {
                Id = userId,
                UserName = userViewModel.Username,
                Password = userViewModel.Password,
                Email = userViewModel.Email,
                Firstname = userViewModel.FirstName,
                Lastname = userViewModel.LastName,
            };

            var userCreationResult = await _userManager.CreateAsync(userData, userViewModel.Password);

            if (!userCreationResult.Succeeded)
            {
                foreach (var error in userCreationResult.Errors) { 
                    ModelState.AddModelError("", error.Description); 
                } 
            }

            var user = await _userManager.FindByIdAsync(userId);
            var roleAttributionResult = await _userManager.AddToRoleAsync(user, userViewModel.Email.Contains("@cesi.fr") ? "Administrator" : "User");

            if (!roleAttributionResult.Succeeded)
            {
                foreach (var error in roleAttributionResult.Errors) { 
                    ModelState.AddModelError("", error.Description); 
                } 
            }
            
            
            return View(userViewModel);
        }
    }
}
