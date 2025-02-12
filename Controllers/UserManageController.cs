using CustomUserManmgment.Data;
using CustomUserManmgment.Models;
using CustomUserManmgment.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CustomUserManmgment.Controllers
{
    [Authorize]

    public class UserManageController : Controller
    {
        public UserManageController(UserManager<ApplicationUser> userManager)
        {
            _UserManager = userManager;
        }

        public UserManager<ApplicationUser> _UserManager { get; }
        [HttpGet]
        public async Task<IActionResult> Update()
        {
            var userName = User.Identity?.Name;
            UpdateUserInfoViewModel updateUserInfo = new UpdateUserInfoViewModel();

            var RegisterdUser = await _UserManager.FindByNameAsync(userName);
            if (RegisterdUser != null)
            {
                updateUserInfo.FirstName=RegisterdUser.FirstName;
                updateUserInfo.LastName=RegisterdUser.LastName;
                updateUserInfo.ProfilePicture = RegisterdUser.profilePicture;
            }
            return View(updateUserInfo);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserInfoViewModel updateUserInfo)
        {

            var userName = User.Identity?.Name;
            var RegisterdUser = await _UserManager.FindByNameAsync(userName);
            var profilePicture = Request.Form.Files[0];
            if (profilePicture != null )
            {
                using (var memoryStream = new MemoryStream())
                {
                    await profilePicture.CopyToAsync(memoryStream);
                    byte[] fileData = memoryStream.ToArray();
                    RegisterdUser.profilePicture = fileData;

                }

            }
            if (RegisterdUser != null)
            {
                RegisterdUser.FirstName = updateUserInfo.FirstName;
                RegisterdUser.LastName = updateUserInfo.LastName;
                await _UserManager.UpdateAsync(RegisterdUser);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(updateUserInfo);

            }

        }
    }
}