using FrontToBack.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FrontToBack.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]


    public class RoleController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {

            return View(await _roleManager.Roles.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// Create Role post action
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string role)
        {
            int count = 0;
            if (role == null) return Ok("Role don't Empty");

            var roles = await _roleManager.Roles.ToListAsync();
            foreach (var item in roles)
            {
                if (item.Name.ToLower() == role.ToLower())
                {
                    count++;
                }
            }
            if (count > 0) return Ok("Role is Alredy Exsist");
            var result = await _roleManager.CreateAsync(new IdentityRole { Name = role });
            return RedirectToAction("index");
        }
        /// <summary>
        /// role  delete Action
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(string Id)
        {
            var result = await _roleManager.FindByIdAsync(Id);
            await _roleManager.DeleteAsync(result);

            return RedirectToAction("index");
        }
        /// <summary>
        /// detail role action
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Detail(string Id)
        {
            if (Id == null) return NotFound();
            var result = await _roleManager.FindByIdAsync(Id);
            return View(result);
        }

       
    }
}
