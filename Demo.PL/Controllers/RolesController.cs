using Demo.DAL.Entities;
using Demo.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.PL.Controllers
{
    [Authorize(Roles = "Admin, HR")]
    public class RolesController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RolesController> _logger;

        public RolesController(RoleManager<ApplicationRole> roleManager,
                               UserManager<ApplicationUser> userManager,
                               ILogger<RolesController> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            return View(roles);
        }

        public IActionResult Create()
        {
            return View(new ApplicationRole());
        }
        [HttpPost]

        public async Task<IActionResult> Create(ApplicationRole role)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(role);

                if(result.Succeeded)
                    return RedirectToAction("Index");

                foreach (var error in result.Errors)
                {
                    _logger.LogError(error.Description);
                    ModelState.AddModelError("", error.Description);

                }
            }
            return View(role);
        }

        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id is null)
                return NotFound();

            var user = await _roleManager.FindByIdAsync(id);

            if (user is null)
                return NotFound();

            return View(user);
        }

        public async Task<IActionResult> Update(string id)
        {
            return await Details(id, "Update");
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, ApplicationRole applicationUserRole)
        {
            if (id != applicationUserRole.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var user = await _roleManager.FindByIdAsync(id);

                user.Name = applicationUserRole.Name;
                user.NormalizedName = applicationUserRole.Name.ToUpper();

                var result = await _roleManager.UpdateAsync(user);

                if (result.Succeeded)
                    return RedirectToAction("Index");

                foreach (var error in result.Errors)
                {
                    _logger.LogError(error.Description);
                    ModelState.AddModelError("", error.Description);

                }
            }
            return View(applicationUserRole);
        }


        public async Task<IActionResult> Delete(string id, ApplicationUser applicationUser)
        {

            if (ModelState.IsValid)
            {
                var user = await _roleManager.FindByIdAsync(id);

                if (user is null)
                    return NotFound();

                var result = await _roleManager.DeleteAsync(user);

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

        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role is null) 
                return NotFound();

            var usersInRole = new List<UserInRoleViewModel>();

            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userInRole = new UserInRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                };

                if(await _userManager.IsInRoleAsync(user, role.Name))
                    userInRole.IsSelected = true;
                else
                    userInRole.IsSelected = false;

                usersInRole.Add(userInRole);
            }
            return View(usersInRole);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId, List<UserInRoleViewModel> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role is null)
                return NotFound();

            ViewBag.RoleId = roleId;

            if (ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appUser = await _userManager.FindByIdAsync(user.UserId);

                    if (appUser != null)
                    {
                        if(user.IsSelected && !(await _userManager.IsInRoleAsync(appUser, role.Name)))
                            await _userManager.AddToRoleAsync(appUser, role.Name);
                        else if(!user.IsSelected && await _userManager.IsInRoleAsync(appUser, role.Name))
                            await _userManager.RemoveFromRoleAsync(appUser, role.Name);
                    }
                }
                return RedirectToAction("Update", new { id = roleId });
            }
            return View(User);
        }
    }
}
