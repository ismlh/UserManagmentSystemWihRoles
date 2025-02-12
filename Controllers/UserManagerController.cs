using CustomUserManmgment.Models;
using CustomUserManmgment.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomUserManmgment.Controllers
{
    [Authorize(Roles = "Admin")]

    public class UserManagerController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserManagerController(UserManager<ApplicationUser> userManager,
                                    RoleManager<IdentityRole> roleManager,
                                    SignInManager<ApplicationUser> signInManager)
        {
            
            _userManager = userManager;
            _roleManager = roleManager;
            this.signInManager = signInManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userViewModels.Add(new UserViewModel
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Roles = roles
                });
            }
            return View(userViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Roles=await _roleManager.Roles.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RigsterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var isExistUser= await _userManager.FindByNameAsync(viewModel.UserName);
                if (isExistUser == null) {
                    var user = new ApplicationUser
                    {
                        UserName = viewModel.UserName,
                        Email = viewModel.UserEmail,
                        FirstName = viewModel.FirstName,
                        LastName = viewModel.LastName,
                    };
                    var result = await _userManager.CreateAsync(user, viewModel.Password);
                    if (result.Succeeded)
                    {
                        if(viewModel.Roles.Count>0)
                        {
                            foreach (var Role in viewModel.Roles)
                            {
                                await _userManager.AddToRoleAsync(user, Role);

                            }
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            ViewBag.Roles = await _roleManager.Roles.ToListAsync();
            return View(viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Update(string UserName)
        {
            var UserData = new UpdateUserDataByAdminViewModel();
            ViewBag.Roles = await _roleManager.Roles.ToListAsync();
            var user = await _userManager.FindByNameAsync(UserName);
            if (user != null)
            {
                var userRoles= await _userManager.GetRolesAsync(user);
                 UserData=new UpdateUserDataByAdminViewModel { FirstName=user.FirstName, 
                    LastName=user.LastName,
                    UserEmail=user.Email,
                    UserName=user.UserName,
                    Roles= (List<string>)userRoles
                };

            }
            return View(UserData);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserDataByAdminViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var isExistUser = await _userManager.FindByNameAsync(viewModel.UserName);
                if (isExistUser != null)
                {
                    isExistUser.UserName = viewModel.UserName;
                    isExistUser.FirstName = viewModel.FirstName;
                    isExistUser.LastName = viewModel.LastName;
                    isExistUser.Email = viewModel.UserEmail;
                    var result = await _userManager.UpdateAsync(isExistUser);
                    if (result.Succeeded)
                    {
                        if (viewModel.Roles?.Count > 0)
                        {
                            foreach (var Role in viewModel.Roles)
                            {
                                await _userManager.AddToRoleAsync(isExistUser, Role);

                            }
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            ViewBag.Roles = await _roleManager.Roles.ToListAsync();
            return View(viewModel);
        }


        public async Task<IActionResult> Delete(string UserName)
        {
            var user=await _userManager.FindByNameAsync(UserName);
            if (user != null)
               await _userManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }
    }
}
