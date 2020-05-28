using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestGenerator.Model.Entities;
using TestGenerator.Web.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace TestGenerator.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {    
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
 
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegistrationViewModel userViewModel)
        {
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

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
            var roleAttributionResult = await _userManager.AddToRoleAsync(user, userViewModel.Email.Contains("@cesi.fr") ? "Admin" : "User");

            if (!roleAttributionResult.Succeeded)
            {
                foreach (var error in roleAttributionResult.Errors) { 
                    ModelState.AddModelError("", error.Description); 
                } 
            }
            
            
            return View(userViewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(UserLoginViewModel model)
        {
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Global", "Identifiants erronés.");
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("Global", "Identifiants erronés.");
                return View(model);
            }

            var loginResult = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, true, false);

            if (loginResult == SignInResult.Failed)
            {
                ModelState.AddModelError("Global", "Identifiants erronés.");
            } else if (loginResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
        

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "User");
        }
    }
}
