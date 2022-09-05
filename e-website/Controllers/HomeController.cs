using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using e_website.Context;
using e_website.Models;
using System.Security.Cryptography;
using System.Text;
using User = e_website.Context.User;
using PagedList;
//using Product = e_website.Context.Product;

namespace e_website.Controllers
{
    public class HomeController : Controller
    {
        qlbhEntities objqlbhEntities = new qlbhEntities();

        

        public ActionResult Index()
        {
            List<Category> lstCate = new List<Category>();
            lstCate = objqlbhEntities.Categories.ToList();

            List<Product> lstPro = new List<Product>();
            lstPro = objqlbhEntities.Products.ToList();

            HomeModel objHomeModel = new HomeModel();
            objHomeModel.ListCategory = lstCate;
            objHomeModel.ListProduct = lstPro;

            return View(objHomeModel);
        }
        
        

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                var check = objqlbhEntities.Users.FirstOrDefault(s => s.Email == user.Email);
                if (check == null)
                {
                    user.Password = GetMD5(user.Password);
                    objqlbhEntities.Configuration.ValidateOnSaveEnabled = false;
                    objqlbhEntities.Users.Add(user);
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

        //Mã hoá mật khẩu
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
                    Session["LastName"] =  data.FirstOrDefault().LastName;
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


        //Logout
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