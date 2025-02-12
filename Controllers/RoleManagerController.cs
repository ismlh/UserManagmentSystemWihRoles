using CustomUserManmgment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomUserManmgment.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoleManagerController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public RoleManagerController(RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> userManager)
        {
            this._roleManager = roleManager;
            this.userManager = userManager;
        }
        public async Task< IActionResult> Index()
        {
            ViewBag.isCoreect = true;

            return View(await _roleManager.Roles.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Create([FromQuery]string Name) {
            if (string.IsNullOrEmpty(Name)) {
                ViewBag.isCoreect = false;
            }
            var Role=new IdentityRole() {Name = Name};
            var result= await _roleManager.CreateAsync(Role);
            if (!result.Succeeded) {
                ViewBag.isCoreect = false;
            }
            return RedirectToAction("Index");
        }
    }
}
