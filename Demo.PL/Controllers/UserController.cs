using Demo.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserController> _logger;

        public UserController(UserManager<ApplicationUser> userManager,
                               ILogger<UserController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
        public async Task<IActionResult> Index(string searchValue = "")
        {
            List<ApplicationUser> users;
            if (string.IsNullOrEmpty(searchValue))
                users = await _userManager.Users.ToListAsync();
            else
                users = await _userManager.Users
                        .Where(user => user.Email.Trim().ToLower().Contains(searchValue.Trim().ToLower())).ToListAsync();  
            
            return View(users);
        }

        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if(id is null)
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);

            if(user is null)
                return NotFound();

            return View(user);
        }

        public async Task<IActionResult> Update(string id)
        {
            return await Details(id, "Update");
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, ApplicationUser applicationUser)
        {
            if(id != applicationUser.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);

                user.UserName = applicationUser.UserName;
                user.NormalizedUserName = applicationUser.UserName.ToUpper();

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                    return RedirectToAction("Index");

                foreach (var error in result.Errors)
                {
                    _logger.LogError(error.Description);
                    ModelState.AddModelError("", error.Description);

                }
            }
            return View(applicationUser);
        }


        public async Task<IActionResult> Delete(string id, ApplicationUser applicationUser)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user is null)
                    return NotFound();

                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                    return RedirectToAction("Index");

                foreach (var error in result.Errors)
                {
                    _logger.LogError(error.Description);
                    ModelState.AddModelError("", error.Description);

                }
            }
            return RedirectToAction("Index");
        }

    }
}
