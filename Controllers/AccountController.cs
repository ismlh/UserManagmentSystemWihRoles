using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CustomUserManmgment.Models;
using CustomUserManmgment.ViewModels;

namespace CustomUserManmgment.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager
            ,SignInManager<ApplicationUser> signInManager
            ,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            this.signInManager = signInManager;
            this._roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }
        [HttpPost]
        public async Task<IActionResult> Register(RigsterViewModel viewModel)
        {
            if (!ModelState.IsValid) {
                var user = new ApplicationUser
                {
                    UserName = viewModel.UserName,
                    Email = viewModel.UserEmail,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                };
                var result=await _userManager.CreateAsync(user,viewModel.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");

                    await signInManager.SignInAsync(user,false);
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            
            }
            return View(viewModel);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid UserName Or Password");
                    return View(model);
                }
                else
                {
                  var result=  await signInManager.PasswordSignInAsync(user,model.Password,false,false);
                    if (result.Succeeded)
                    {

                        return RedirectToAction("Index", "Home");

                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid UserName Or Password");
                    }
                }

            }
            return View(model);
        }
    }
    
}
