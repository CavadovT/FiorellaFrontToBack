using FrontToBack.DAL;
using FrontToBack.Extentions;
using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public async Task<IActionResult> Index(int page = 1, int take = 5)
        {
            ViewBag.Page=page;
            List<Product> products = await _context.Products.Include(p => p.Category).Skip((page - 1) * take).Take(take).ToListAsync();
            int pageCount = await PageCount(take);
            PaginationVM<Product> pagVM = new PaginationVM<Product>(products, await PageCount(take), page);
            return View(pagVM);
        }

        private async Task<int> PageCount(int take)
        {
            List<Product> products = await _context.Products.ToListAsync();
            if (products.Count() % take == 0)
            {
                return (int)Math.Ceiling((decimal)(products.Count() / take));
            }
            else 
            {
                return (int)Math.Ceiling((decimal)(products.Count() / take)) + 1;
            }
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


        public async Task<IActionResult> Update(int? Id)
        {
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
            if (Id == null) return NotFound();
            Product dbProd = await _context.Products.FindAsync(Id);
            if (dbProd == null) return BadRequest();
            return View(dbProd);


        }
        /// <summary>
        /// burada sekli default qoya bilmez admin gerek updateye girdise deyise yeni sekil add ede 
        /// amma nezere aldim ki bu seklin evvelkini silsin imgden
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Product product)
        {
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
            if (!ModelState.IsValid)
            {
                return View();
            }
            Product dbProd = await _context.Products.FindAsync(product.Id);
            if (dbProd == null)
            {
                return View();
            }
            else
            {
                Product dbProductName = await _context.Products.FirstOrDefaultAsync(p => p.Name.Trim().ToLower() == product.Name.Trim().ToLower());
                if (dbProductName != null)
                {
                    if (dbProductName.Name.Trim().ToLower() != dbProd.Name.Trim().ToLower())
                    {
                        ModelState.AddModelError("Name", "with this name product allready exist!!!");
                        return View();
                    }
                }
                if (product.Photo == null)
                {
                    dbProd.ImgUrl = dbProd.ImgUrl;
                }
                else
                {
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
                    string oldPhoto = dbProd.ImgUrl;
                    string path = Path.Combine(_env.WebRootPath, "img", oldPhoto);

                    Helper.Helper.DeleteImage(path);

                    dbProd.ImgUrl = product.Photo.SaveImage(_env, "img");

                }

                dbProd.Name = product.Name;
                dbProd.Price = product.Price;
                dbProd.CategoryId = product.CategoryId;
                dbProd.Count = product.Count;
                await _context.SaveChangesAsync();

            }
            return RedirectToAction("index");
        }
    }
}
