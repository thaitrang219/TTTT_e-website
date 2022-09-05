using e_website.Context;
using e_website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace e_website.Controllers
{
    public class PaymentController : Controller
    {
        qlbhEntities objqlbhEntities = new qlbhEntities();
        // GET: /Pay/
        public ActionResult Index()
        {
            if (Session["idUser"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                //Lấy thông tin từ giỏ hàng từ biến sesstion
                var lstCart = (List<CartModel>)Session["cart"];
                //gắn dữ liệu cho Order
                Order objOrder = new Order();
                objOrder.Name = "DonHang" + DateTime.Now.ToString("yyyyMMddHHmmss");
                objOrder.UserId = int.Parse(Session["idUser"].ToString());
                objOrder.CreatedOnUtc = DateTime.Now;
                objOrder.Status = 1;
                objqlbhEntities.Orders.Add(objOrder);
                //Lưu thông tin dữ liệu vào bảng Order
                objqlbhEntities.SaveChanges();

                //Lấy OrderId vừa mới tạo để lưu vào bảng Orderdetail
                int intOrderId = objOrder.Id;
                List<OrderDetail> lstOrderDetail = new List<OrderDetail>();
                foreach (var item in lstCart)
                {
                    OrderDetail obj = new OrderDetail();
                    obj.Quantity = item.Quantity;
                    obj.OrderId = intOrderId;
                    obj.ProductId = item.Product.Id;
                    lstOrderDetail.Add(obj);
                }
                objqlbhEntities.OrderDetails.AddRange(lstOrderDetail);
                objqlbhEntities.SaveChanges();
            }
            return View();
        }
    }
}