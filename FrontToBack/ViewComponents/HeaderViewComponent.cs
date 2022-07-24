using FrontToBack.DAL;
using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public HeaderViewComponent(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.User = "";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.User = (await _userManager.FindByNameAsync(User.Identity.Name)).UserName;

            }
            
            ViewBag.totalCount = 0;
            ViewBag.totalPrice = 0;
            string basket = Request.Cookies["basket"];
            if (basket != null)
            {
                List<ProductReturnVM> prodList = JsonConvert.DeserializeObject<List<ProductReturnVM>>(basket);

                foreach (var item in prodList)
                {
                    Product dbProd = _context.Products.FirstOrDefault(p => p.Id == item.Id);
                    item.Price = dbProd.Price;
                    item.ImgUrl = dbProd.ImgUrl;
                    item.Name = dbProd.Name;
                    item.CategoryId = dbProd.CategoryId;
                }
                int totalCount = 0;
                double totalPrice = 0;
                foreach (var item in prodList)
                {
                    totalCount += item.ProductCount;
                    totalPrice += (item.Price * item.ProductCount);
                }
                ViewBag.totalCount = totalCount;
                ViewBag.totalPrice = totalPrice;
            }
            else
            {
                ViewBag.totalCount = 0;
                ViewBag.totalPrice = 0;
            }
            Bio bio = _context.Bios.FirstOrDefault();
            return View(await Task.FromResult(bio));

        }
    }
}
