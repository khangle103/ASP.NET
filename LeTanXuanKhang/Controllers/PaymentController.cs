using LeTanXuanKhang.Context;
using LeTanXuanKhang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeTanXuanKhang.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment
        public ActionResult Index()
        {
            WebsiteBanHangEntities4 WebsiteBanHangEntities4 = new WebsiteBanHangEntities4();
            if (Session["idUser"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                // lấy thông tin từ giỏ hàng từ biến session  
                var lstCart = (List<CartModel>)Session["cart"];
                // gán dữ liệu cho Order  
                Order objOrder = new Order();
                objOrder.Name = "DonHang-" + DateTime.Now.ToString("yyyyMMddHHmmss");
                objOrder.UserId = int.Parse(Session["idUser"].ToString());
                objOrder.CreatedOnUtc = DateTime.Now;
                objOrder.Status = 1;
                WebsiteBanHangEntities4.Orders.Add(objOrder);
                // lưu thông tin dữ liệu vào bảng order  
                WebsiteBanHangEntities4.SaveChanges();
                // Lấy OrderId vừa mới tạo để lưu vào bảng OrderDetail.  
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

                WebsiteBanHangEntities4.OrderDetails.AddRange(lstOrderDetail);
                WebsiteBanHangEntities4.SaveChanges();
            }

            return View();
        }
    }
}