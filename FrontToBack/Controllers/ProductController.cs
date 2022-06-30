using FrontToBack.DAL;
using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FrontToBack.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        //private readonly IMapper _mapper;

        //public ProductController(IMapper mapper)
        //{
        //    _mapper = mapper;
        //}

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Product> products = _context.Products.Include(p => p.Category).ToList();

            return View(products);
        }
        public IActionResult LoadMore(int skip)
        {
            #region MaperAuto
            // private readonly IMapper _mapper;
            //public ProductController(IMapper mapper)
            //{
            //    _mapper = mapper;
            //}
            //public IActionResult LoadMore() 
            //{ 
            //var products=_context.Product.ToList();
            //var model = _mapper.Map<ProductDto>(products);
            //return Json(model);
            //burada Dtonu yaratmaq evvelden lazimdi ve basqa isler aparmaq lazimdi
            //uzatmamaq ucun sade yazdim ozum ucun
            //}
            #endregion
            #region Json for Include
            //  List<Product> products = _context.Products.Include(p => p.Category).Skip(2).Take(2).ToList();
            //List<ProductReturnVM> productReturnVMs = new List<ProductReturnVM>();
            //foreach (Product product in products) 
            //{ 
            //    ProductReturnVM productReturnVM = new ProductReturnVM();
            //    productReturnVM.Id = product.Id;
            //    productReturnVM.CategoryId = product.CategoryId;
            //    productReturnVM.Price= product.Price;
            //    productReturnVM.Name= product.Name;
            //    productReturnVM.ImgUrl=product.ImgUrl;
            //    productReturnVM.Category = product.Category.CategoryName;
            //    productReturnVMs.Add(productReturnVM);

            //}



            //List<ProductReturnVM> productReturns = _context.Products.Select(p => new ProductReturnVM
            //{
            //    Id = p.Id,
            //    Name = p.Name,
            //    Price = p.Price,
            //    Category = p.Category.CategoryName,
            //    CategoryId = p.CategoryId,
            //    ImgUrl = p.ImgUrl,

            //}).ToList();
            //return Json(productReturns);

            #endregion
            List<Product> products=_context.Products.Skip(skip).Take(2).Include(p=>p.Category).ToList();
            return PartialView("_LoadMorePartial", products);

        }
        public IActionResult Detail(int? id, string name)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product dbProduct = _context.Products.FirstOrDefault(p => p.Id == id);
            if (dbProduct == null)
            {
                return NotFound();
            }
            return View(dbProduct);

        }

    }
}
