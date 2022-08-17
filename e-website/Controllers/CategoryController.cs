using e_website.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace e_website.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        qlbhEntities objqlbhEntities = new qlbhEntities();
        public ActionResult Index()
        {
            var lstCategory = objqlbhEntities.Categories.ToList();
            return View(lstCategory);
        }
        public ActionResult ProductCategory(int Id)
        {
            var listProduct = objqlbhEntities.Products.Where(n => n.CategoryId == Id).ToList();
            return View(listProduct);   
        }
    }
}