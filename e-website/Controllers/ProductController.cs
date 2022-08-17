using e_website.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace e_website.Controllers
{
    public class ProductController : Controller
    {
        qlbhEntities objqlbhEntities = new qlbhEntities();


        // GET: Product
        public ActionResult Detail(int Id)
        {
            var objProducts = objqlbhEntities.Products.Where(n => n.Id== Id).FirstOrDefault();
            return View(objProducts);
        }
    }
}