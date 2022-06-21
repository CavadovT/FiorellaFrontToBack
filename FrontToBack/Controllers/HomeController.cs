using FrontToBack.DAL;
using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FrontToBack.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM();
            homeVM.Sliders = _context.Sliders.ToList();
            homeVM.SliderContent = _context.SliderContents.FirstOrDefault();
            homeVM.Categories = _context.Categories.ToList();
            homeVM.Products = _context.Products.Include(p => p.Category).ToList();
            return View(homeVM);
        }
        public IActionResult Detail(int? id, string name)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product dbProduct=_context.Products.FirstOrDefault(p => p.Id == id);
            if(dbProduct==null)
            {
                return NotFound();
            }
            return View(dbProduct);

        }
    }
}
