using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FrontToBack.Controllers
{
    public class BasketController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddItem() 
        {
            HttpContext.Session.SetString("name","Tural");
            return Content("");
        }
        public IActionResult ShowItem()
        {
            HttpContext.Session.Get("name");
                return Content("");
        }
    }
}
