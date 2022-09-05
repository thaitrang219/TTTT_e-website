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
        qlbhEntities objqlbhEntities = new qlbhEntities();
        //
        // GET: /Category/
        public ActionResult Index()
        {
            var lstCategory = objqlbhEntities.Categories.ToList();
            return View(lstCategory);
        }
        public ActionResult ProductCategory(int Id)
        {
            var lstProduct = objqlbhEntities.Products.Where(n => n.CategoryId == Id).ToList();
            return View(lstProduct);
        }
    }
}