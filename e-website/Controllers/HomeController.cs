using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using e_website.Context;
using e_website.Models;
using System.Security.Cryptography;
using System.Text;


namespace e_website.Controllers
{
    public class HomeController : Controller
    {
        qlbhEntities objqlbhEntities = new qlbhEntities();
        //private List<Category> stCategories;

        public ActionResult Index()
        {
            HomeModel objHomeModel = new HomeModel();
            objHomeModel.ListCategory = objqlbhEntities.Categories.ToList();
            objHomeModel.ListProduct = objqlbhEntities.Products.ToList();
            return View(objHomeModel);
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(User _user)
        {
            if (ModelState.IsValid)
            {
                var check = objqlbhEntities.Users.FirstOrDefault(s => s.Email == _user.Email);
                if (check == null)
                {
                    _user.Password = GetMD5(_user.Password);
                    objqlbhEntities.Configuration.ValidateOnSaveEnabled = false;
                    objqlbhEntities.Users.Add(_user);
                    objqlbhEntities.SaveChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.error = "Email đã tồn tại";
                    return View("Register");
                }
            }
            return View("Index");
        }
        

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {


                var f_password = GetMD5(password);
                var data = objqlbhEntities.Users.Where(s => s.Email.Equals(email) && s.Password.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["FullName"] = data.FirstOrDefault().FirstName + " " + data.FirstOrDefault().LastName;
                    Session["Email"] = data.FirstOrDefault().Email;
                    Session["idUser"] = data.FirstOrDefault().Id;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Đăng nhập thất bại";
                    return RedirectToAction("Login");
                }
            }
            return View();
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

        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}