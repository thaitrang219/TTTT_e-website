using e_website;
using e_website.Context;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using static e_website.Common;

namespace e_website.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        qlbhEntities objqlbhEntities = new qlbhEntities();
        // GET: Admin/Product
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var lstProduct = new List<Product>();
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            if (!string.IsNullOrEmpty(SearchString))
            {
                lstProduct = objqlbhEntities.Products.Where(n => n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                lstProduct = objqlbhEntities.Products.ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            lstProduct = lstProduct.OrderByDescending(n => n.Id).ToList();
            return View(lstProduct.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            Common objCommon = new Common();
            //Lấy dữ liệu danh mục dưới DB
            var lstCat = objqlbhEntities.Categories.ToList();
            //Convert sang select list dạng value, text
            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            DataTable dtCategory = converter.ToDataTable(lstCat);
            ViewBag.ListCategory = objCommon.ToSelectList(dtCategory, "Id", "Name");

            //Lấy dữ liệu thương hiệu dưới DB
            var lstBrand = objqlbhEntities.Brands.ToList();
            DataTable dtBrand = converter.ToDataTable(lstBrand);
            //Convert sang select list dạng value, text
            ViewBag.ListBrand = objCommon.ToSelectList(dtBrand, "Id", "Name");

            //Loại sản phẩm
            List<ProductType> lstProductType = new List<ProductType>();
            ProductType objProductType = new ProductType();
            objProductType.Id = 1;
            objProductType.Name = "Giảm giá sốc";
            lstProductType.Add(objProductType);

            objProductType = new ProductType();
            objProductType.Id = 2;
            objProductType.Name = "Đề xuất";
            lstProductType.Add(objProductType);

            DataTable dtProductType = converter.ToDataTable(lstProductType);
            ViewBag.ProductType = objCommon.ToSelectList(dtProductType, "Id", "Name");

            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(Product product)
        {
            
            if (ModelState.IsValid)//Lưu ý
            {
                try
                {
                    if (product.ImageUpLoad != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(product.ImageUpLoad.FileName);
                        string extension = Path.GetExtension(product.ImageUpLoad.FileName);
                        fileName = fileName + extension;
                        product.Avatar = fileName;
                        product.ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/images/product"), fileName));
                    }
                    product.CreatedOnUtc = DateTime.Now;
                    objqlbhEntities.Products.Add(product);
                    objqlbhEntities.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            return View(product);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var product = objqlbhEntities.Products.Where(n => n.Id == id).FirstOrDefault();
            return View(product);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var product = objqlbhEntities.Products.Where(n => n.Id == id).FirstOrDefault();
            return View(product);
        }
        [HttpPost]
        public ActionResult Delete(Product objPro)
        {
            var objProduct = objqlbhEntities.Products.Where(n => n.Id == objPro.Id).FirstOrDefault();
            objqlbhEntities.Products.Remove(objProduct);
            objqlbhEntities.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Common objCommon = new Common();
            //Lấy dữ liệu danh mục dưới DB
            var lstCat = objqlbhEntities.Categories.ToList();
            //Convert sang select list dạng value, text
            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            DataTable dtCategory = converter.ToDataTable(lstCat);
            ViewBag.ListCategory = objCommon.ToSelectList(dtCategory, "Id", "Name");

            //Lấy dữ liệu thương hiệu dưới DB
            var lstBrand = objqlbhEntities.Brands.ToList();
            DataTable dtBrand = converter.ToDataTable(lstBrand);
            //Convert sang select list dạng value, text
            ViewBag.ListBrand = objCommon.ToSelectList(dtBrand, "Id", "Name");

            //Loại sản phẩm
            List<ProductType> lstProductType = new List<ProductType>();
            ProductType objProductType = new ProductType();
            objProductType.Id = 1;
            objProductType.Name = "Giảm giá sốc";
            lstProductType.Add(objProductType);

            objProductType = new ProductType();
            objProductType.Id = 2;
            objProductType.Name = "Đề xuất";
            lstProductType.Add(objProductType);

            DataTable dtProductType = converter.ToDataTable(lstProductType);
            ViewBag.ProductType = objCommon.ToSelectList(dtProductType, "Id", "Name");
            var product = objqlbhEntities.Products.Where(n => n.Id == id).FirstOrDefault();
            return View(product);
        }

        [HttpPost] 
        [ValidateInput(false)]
        public ActionResult Edit(int id, Product objProduct)
        {
            if (objProduct.ImageUpLoad != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpLoad.FileName);
                string extension = Path.GetExtension(objProduct.ImageUpLoad.FileName);
                fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                objProduct.Avatar = fileName;
                objProduct.ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/images/product"), fileName));
            }
            objqlbhEntities.Entry(objProduct).State = EntityState.Modified;
            objqlbhEntities.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}