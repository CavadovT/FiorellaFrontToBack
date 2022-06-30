using FrontToBack.DAL;
using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FrontToBack.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;

        public BasketController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> AddItem(int? Id)
        {
            // HttpContext.Session.SetString("name","Tural");
            // Response.Cookies.Append("group", "p322",new CookieOptions { MaxAge=TimeSpan.FromDays(5)});
            if (Id == null) return NotFound();

            Product dbProd = await _context.Products.FindAsync(Id);

            if (dbProd == null) return NotFound();
            List<ProductReturnVM> products;

            string basket = Request.Cookies["basket"];

            if (basket == null)
            {
                products = new List<ProductReturnVM>();
            }
            else
            {
                products = JsonConvert.DeserializeObject<List<ProductReturnVM>>(basket);
            }

            ProductReturnVM IsExist = products.Find(p => p.Id == Id);

            if (IsExist == null)
            {
                ProductReturnVM prodVM = new ProductReturnVM
                {
                    Id = dbProd.Id,
                    Name = dbProd.Name,
                    Price = dbProd.Price,
                    ImgUrl = dbProd.ImgUrl,
                    CategoryId = dbProd.CategoryId,
                    //Category = dbProd.Category.CategoryName,
                    ProductCount = 1

                };
                products.Add(prodVM);
            }
            else 
            {
                IsExist.ProductCount++;
            
            }
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products), new CookieOptions { MaxAge = TimeSpan.FromDays(5) });
            return RedirectToAction("index", "product");
        }

        public IActionResult ShowItem()
        {
            //string name= HttpContext.Session.GetString("name");


            string basket = Request.Cookies["basket"];
            List<ProductReturnVM> prods = JsonConvert.DeserializeObject<List<ProductReturnVM>>(basket);
            return View(prods);
        }
    }
}
