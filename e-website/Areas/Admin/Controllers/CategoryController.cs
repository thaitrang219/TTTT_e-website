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
    public class CategoryController : Controller
    {
        qlbhEntities objqlbhEntities = new qlbhEntities();
        // GET: Admin/Category
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var lstCategory = new List<Category>();
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
                lstCategory = objqlbhEntities.Categories.Where(n => n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                lstCategory = objqlbhEntities.Categories.ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            lstCategory = lstCategory.OrderByDescending(n => n.Id).ToList();
            return View(lstCategory.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var product = objqlbhEntities.Categories.Where(n => n.Id == id).FirstOrDefault();
            return View(product);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var product = objqlbhEntities.Categories.Where(n => n.Id == id).FirstOrDefault();
            return View(product);
        }
        [HttpPost]
        public ActionResult Delete(Category objCat)
        {
            var objCategory = objqlbhEntities.Categories.Where(n => n.Id == objCat.Id).FirstOrDefault();
            objqlbhEntities.Categories.Remove(objCategory);
            objqlbhEntities.SaveChanges();
            return RedirectToAction("Index");
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

            //Loại danh mục
            List<CategoryType> lstCategoryType = new List<CategoryType>();
            CategoryType objCategoryType = new CategoryType();
            objCategoryType.Id = 1;
            objCategoryType.Name = "Danh mục phổ biến";
            lstCategoryType.Add(objCategoryType);

            objCategoryType = new CategoryType();
            objCategoryType.Id = null;
            objCategoryType.Name = "Danh mục";
            lstCategoryType.Add(objCategoryType);

            DataTable dtCategoryType = converter.ToDataTable(lstCategoryType);
            ViewBag.CategoryType = objCommon.ToSelectList(dtCategoryType, "Id", "Name");

            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)//Lưu ý
            {
                try
                {
                    if (category.ImageUpLoad != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(category.ImageUpLoad.FileName);
                        string extension = Path.GetExtension(category.ImageUpLoad.FileName);
                        fileName = fileName + extension;
                        category.Avatar = fileName;
                        category.ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/images/category"), fileName));
                    }
                    category.CreatedOnUtc = DateTime.Now;
                    objqlbhEntities.Categories.Add(category);
                    objqlbhEntities.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch
                {
                    return RedirectToAction("Index");
                }
            }
            return View(category);
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

            //Loại danh mục
            List<CategoryType> lstCategoryType = new List<CategoryType>();
            CategoryType objCategoryType = new CategoryType();
            objCategoryType.Id = 1;
            objCategoryType.Name = "Danh mục phổ biến";
            lstCategoryType.Add(objCategoryType);

            objCategoryType = new CategoryType();
            objCategoryType.Id = null;
            objCategoryType.Name = "Danh mục";
            lstCategoryType.Add(objCategoryType);

            DataTable dtCategoryType = converter.ToDataTable(lstCategoryType);
            ViewBag.CategoryType = objCommon.ToSelectList(dtCategoryType, "Id", "Name");

            var category = objqlbhEntities.Categories.Where(n => n.Id == id).FirstOrDefault();
            return View(category);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(int id, Category objCategory)
        {
            if (objCategory.ImageUpLoad != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objCategory.ImageUpLoad.FileName);
                string extension = Path.GetExtension(objCategory.ImageUpLoad.FileName);
                fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                objCategory.Avatar = fileName;
                objCategory.ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/images/product"), fileName));
            }
            objqlbhEntities.Entry(objCategory).State = EntityState.Modified;
            objqlbhEntities.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}