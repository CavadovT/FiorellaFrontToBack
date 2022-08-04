using FrontToBack.DAL;
using FrontToBack.Models;
using FrontToBack.SignalRChat;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<HubChat> _hub;
        public HomeController(AppDbContext context, IHubContext<HubChat> hub)
        {
            _context = context;
            _hub = hub;
        }
        public IActionResult Index()
        {
            

            HomeVM homeVM = new HomeVM();
            homeVM.Sliders = _context.Sliders.ToList();
            homeVM.SliderContent = _context.SliderContents.FirstOrDefault();
            homeVM.Categories = _context.Categories.ToList();
            homeVM.Products = _context.Products.Include(p => p.Category).ToList();
            homeVM.About = _context.Abouts.FirstOrDefault();
            homeVM.Icons = _context.Icons.ToList();
            homeVM.Blogs=_context.Blogs.ToList();
            homeVM.Experts=_context.Experts.ToList();
            homeVM.Says=_context.Says.ToList();
            homeVM.Instagrams=_context.Instagrams.ToList();

            return View(homeVM);
        }
        
        public IActionResult SearchProduct(string search)
        {
            List<Product> products = _context.Products
                    .Include(p => p.Category)
                    .OrderBy(p => p.Id)
                    .Where(p => p.Name.ToLower()
                    .Contains(search.ToLower()))
                    .Take(search.Length*3)
                    .ToList();
            return PartialView("_SearchPartial", products);
        }
        public async Task< IActionResult> Chat()
        {
            List<AppUser> users = await _context.Users.ToListAsync();
            return View(users);
        }
        public async Task<IActionResult> PrivateSend(string id) 
        { 
            var user = await _context.Users.FirstOrDefaultAsync(u=>u.Id==id);
            await _hub.Clients.Client(user.ConnectedId).SendAsync("PrivateMessage");
            return RedirectToAction("chat");
        }
    }
}
