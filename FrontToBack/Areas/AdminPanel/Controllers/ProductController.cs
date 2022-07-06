using FrontToBack.DAL;
using FrontToBack.Extentions;
using FrontToBack.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;

namespace FrontToBack.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        /// <summary>
        /// Home view
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.Include(p => p.Category).ToListAsync());
        }
        /// <summary>
        /// Detail View
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Detail(int? Id)
        {
            if (Id == null) return NotFound();
            Product dbProduct = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == Id);
            if (dbProduct == null) return NotFound();
            return View(dbProduct);

        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");

            if (product.Photo == null)
            {
                ModelState.AddModelError("Photo", "don't leave it blank!!!");
                return View();
            }
            if (!product.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Choose the photo");
                return View();
            }
            if (product.Photo.ValidSize(200))
            {
                ModelState.AddModelError("Photo", "oversize");
                return View();
            }


            Product newProduct = new Product
            {
                Name = product.Name,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Count = product.Count,
                ImgUrl = product.Photo.SaveImage(_env, "img"),

            };

            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }

        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null) return NotFound();
            Product dbProd = await _context.Products.FindAsync(Id);
            if (dbProd == null) return NotFound();

            string path = Path.Combine(_env.WebRootPath, "img", dbProd.ImgUrl);

            Helper.Helper.DeleteImage(path);

            _context.Products.Remove(dbProd);
            await _context.SaveChangesAsync();

            return RedirectToAction("index");
        }


        public async Task<IActionResult> Update() 
        {
        
        }
    }
}
