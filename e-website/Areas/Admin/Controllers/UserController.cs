using e_website.Context;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace e_website.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        qlbhEntities objqlbhEntities = new qlbhEntities();
        // GET: Admin/User
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var lstUser = new List<User>();
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
                lstUser = objqlbhEntities.Users.Where(n => n.FirstName.Contains(SearchString)).ToList();
            }
            else
            {
                lstUser = objqlbhEntities.Users.ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            lstUser = lstUser.OrderByDescending(n => n.Id).ToList();
            return View(lstUser.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)//Lưu ý
            {
                try
                {
                    user.Password = GetMD5(user.Password);
                    objqlbhEntities.Users.Add(user);
                    objqlbhEntities.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch
                {
                    return RedirectToAction("Index");
                }
            }
            return View(user);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var User = objqlbhEntities.Users.Where(n => n.Id == id).FirstOrDefault();
            return View(User);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var User = objqlbhEntities.Users.Where(n => n.Id == id).FirstOrDefault();
            return View(User);
        }
        [HttpPost]
        public ActionResult Delete(User objPro)
        {
            var objUser = objqlbhEntities.Users.Where(n => n.Id == objPro.Id).FirstOrDefault();
            objqlbhEntities.Users.Remove(objUser);
            objqlbhEntities.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var User = objqlbhEntities.Users.Where(n => n.Id == id).FirstOrDefault();
            return View(User);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(int id, User objUser)
        {
            objUser.Password = GetMD5(objUser.Password);
            objqlbhEntities.Entry(objUser).State = EntityState.Modified;
            objqlbhEntities.SaveChanges();
            return RedirectToAction("Index");
        }

        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] formData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(formData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }
            return byte2String;
        }
    }
}