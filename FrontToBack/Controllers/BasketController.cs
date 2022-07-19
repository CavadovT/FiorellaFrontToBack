using FrontToBack.DAL;
using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BasketController(AppDbContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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

            List<ProductReturnVM> prods;
            string basket = Request.Cookies["basket"];
            if (basket != null)
            {
                prods = JsonConvert.DeserializeObject<List<ProductReturnVM>>(basket);
                foreach (var item in prods)
                {
                    Product dbProd = _context.Products.FirstOrDefault(p => p.Id == item.Id);
                    item.Price = dbProd.Price;
                    item.ImgUrl = dbProd.ImgUrl;
                    item.Name = dbProd.Name;
                    item.CategoryId = dbProd.CategoryId;
                }

            }
            else
            {
                prods = new List<ProductReturnVM>();
            }
            return View(prods);
        }
        public IActionResult minusBtn(int id)
        {

            string basket = Request.Cookies["basket"];

            List<ProductReturnVM> prods = JsonConvert.DeserializeObject<List<ProductReturnVM>>(basket);


            ProductReturnVM product = prods.Find(p => p.Id == id);
            if (product.ProductCount > 1)
            {
                product.ProductCount--;
            }
            else
            {
                prods.Remove(product);
            }

            Response.Cookies.Append("basket", JsonConvert.SerializeObject(prods), new CookieOptions { MaxAge = TimeSpan.FromDays(5) });

            return RedirectToAction("ShowItem", "basket");
        }
        public IActionResult plusBtn(int id)
        {
            string basket = Request.Cookies["basket"];

            List<ProductReturnVM> prods = JsonConvert.DeserializeObject<List<ProductReturnVM>>(basket);


            ProductReturnVM product = prods.Find(p => p.Id == id);

            product.ProductCount++;


            Response.Cookies.Append("basket", JsonConvert.SerializeObject(prods), new CookieOptions { MaxAge = TimeSpan.FromDays(5) });

            return RedirectToAction("ShowItem", "basket");
        }
        public IActionResult RemoveItem(int id)
        {
            string basket = Request.Cookies["basket"];

            List<ProductReturnVM> prods = JsonConvert.DeserializeObject<List<ProductReturnVM>>(basket);


            ProductReturnVM product = prods.Find(p => p.Id == id);

            prods.Remove(product);


            Response.Cookies.Append("basket", JsonConvert.SerializeObject(prods), new CookieOptions { MaxAge = TimeSpan.FromDays(5) });

            return RedirectToAction("ShowItem", "basket");
        }

        [HttpPost]
        public async Task<IActionResult> Sale()
        {
            if (User.Identity.IsAuthenticated)
            {
                AppUser user =await _userManager.FindByNameAsync(User.Identity.Name);
                Sale sale = new Sale();
                sale.SaleDate=DateTime.Now;
                sale.AppUserId=user.Id;

                List<ProductReturnVM> basketproducts = JsonConvert.DeserializeObject<List<ProductReturnVM>>(Request.Cookies["basket"]);

                List<SaleProduct> saleProducts = new List<SaleProduct>();
                double total = 0;
                foreach (var product in basketproducts)
                {
                    Product dbProduct= await _context.Products.FindAsync(product.Id);
                    SaleProduct saleProduct= new SaleProduct();
                    saleProduct.ProductId=dbProduct.Id;
                    saleProduct.Count=product.ProductCount;
                    saleProduct.SaleId = sale.Id;
                    saleProducts.Add(saleProduct);
                    total+=product.ProductCount*dbProduct.Price;
                }
                sale.SaleProducts=saleProducts;
                sale.TotalPrice=total;
                await _context.AddAsync(sale);
                await _context.SaveChangesAsync();

                TempData["success"] = "sale successly";

                return RedirectToAction("ShowItem");
            }
            else 
            {
                return RedirectToAction("login", "account");
            }
        }
    }
}
