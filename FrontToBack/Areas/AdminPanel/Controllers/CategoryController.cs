using FrontToBack.DAL;
using FrontToBack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Category> categories = _context.Categories.ToList();
            return View(categories);
        }

        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(/*[FromForm]*/Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            bool ExistNameCategory = _context.Categories.Any(c => c.Name.Trim().ToLower() == category.Name.Trim().ToLower());
            if (ExistNameCategory) 
            {
                ModelState.AddModelError("Name", "with this name category allready exist");
            }

            Category newCategory = new Category
            {
                Name = category.Name,
                Description=category.Description,
            };

            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();

            return RedirectToAction("index");
        }

        /// <summary>
        /// categoriler haqqinda etrafli info methodu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Detail(int? id)
        {
            if (id == null) return NotFound();
            Category category = _context.Categories.Find(id);
            if (category == null) return NotFound();
            return View(category);
        }

        public IActionResult Update(int? id)
        {
            if (id == null) return NotFound();
            Category category = _context.Categories.Find(id);
            if (category == null) return NotFound();
            return View(category);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Category category) 
        {
            if (!ModelState.IsValid) 
            {
                return View();
            }
            Category dbCategory= _context.Categories.FirstOrDefault(c=>c.Id==category.Id);
            if (dbCategory == null) 
            {
                return View(); 
            }
            else
            {
                Category dbCategoryName = _context.Categories.FirstOrDefault(c => c.Name.Trim().ToLower() == category.Name.Trim().ToLower());

                if (dbCategoryName != null)
                {
                    if (dbCategoryName.Name.Trim().ToLower() != dbCategory.Name.Trim().ToLower())
                    {
                        ModelState.AddModelError("Name", "with this name category allready exist");
                        return View();
                    }
                }
                dbCategory.Name = category.Name;
                dbCategory.Description = category.Description;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Category category = _context.Categories.Find(id);
            if (category == null) return NotFound();
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }

    }
}
