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
    public class BrandController : Controller
    {
        qlbhEntities objqlbhEntities = new qlbhEntities();
        // GET: Admin/Brand
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var lstBrand = new List<Brand>();
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
                lstBrand = objqlbhEntities.Brands.Where(n => n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                lstBrand = objqlbhEntities.Brands.ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            lstBrand = lstBrand.OrderByDescending(n => n.Id).ToList();
            return View(lstBrand.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var product = objqlbhEntities.Brands.Where(n => n.Id == id).FirstOrDefault();
            return View(product);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var product = objqlbhEntities.Brands.Where(n => n.Id == id).FirstOrDefault();
            return View(product);
        }
        [HttpPost]
        public ActionResult Delete(Brand objCat)
        {
            var objBrand = objqlbhEntities.Brands.Where(n => n.Id == objCat.Id).FirstOrDefault();
            objqlbhEntities.Brands.Remove(objBrand);
            objqlbhEntities.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(Brand brand)
        {
            if (ModelState.IsValid)//Lưu ý
            {
                try
                {
                    if (brand.ImageUpLoad != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(brand.ImageUpLoad.FileName);
                        string extension = Path.GetExtension(brand.ImageUpLoad.FileName);
                        fileName = fileName + extension;
                        brand.Avatar = fileName;
                        brand.ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/images/brand"), fileName));
                    }
                    brand.CreatedOnUtc = DateTime.Now;
                    objqlbhEntities.Brands.Add(brand);
                    objqlbhEntities.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch
                {
                    return RedirectToAction("Index");
                }
            }
            return View(brand);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var Brand = objqlbhEntities.Brands.Where(n => n.Id == id).FirstOrDefault();
            return View(Brand);
        }

         [HttpPost, ValidateInput(false)]
        public ActionResult Edit(int id, Brand objBrand)
        {
            if (objBrand.ImageUpLoad != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objBrand.ImageUpLoad.FileName);
                string extension = Path.GetExtension(objBrand.ImageUpLoad.FileName);
                fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                objBrand.Avatar = fileName;
                objBrand.ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/images/brand"), fileName));
            }
            objqlbhEntities.Entry(objBrand).State = EntityState.Modified;
            objqlbhEntities.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
