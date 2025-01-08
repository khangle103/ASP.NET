using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeTanXuanKhang.Models;
using LeTanXuanKhang.Context;

namespace LeTanXuanKhang.Controllers
{
    public class CartController : Controller
    {
        // Khai báo đối tượng kết nối với cơ sở dữ liệu
        WebsiteBanHangEntities4 objwebsiteBanHangEntities = new WebsiteBanHangEntities4();

        // GET: Cart
        public ActionResult Cart()
        {
            return View((List< CartModel >) Session["cart"]);
        }

        public ActionResult AddToCart(int id, int quantity)
        {
            if (Session["cart"] == null)
            {
                // Tạo giỏ hàng mới nếu giỏ hàng chưa tồn tại
                List<CartModel> cart = new List<CartModel>();
                cart.Add(new CartModel
                {
                    Product = objwebsiteBanHangEntities.Products.Find(id),
                    Quantity = quantity
                });
                Session["cart"] = cart;
                Session["count"] = 1;
            }
            else
            {
                // Lấy giỏ hàng từ Session
                List<CartModel> cart = (List<CartModel>)Session["cart"];
                // Kiểm tra sản phẩm đã tồn tại trong giỏ hàng chưa
                int index = isExist(id);
                if (index != -1)
                {
                    // Nếu sản phẩm đã tồn tại, tăng số lượng
                    cart[index].Quantity += quantity;
                }
                else
                {
                    // Nếu chưa tồn tại, thêm sản phẩm mới
                    cart.Add(new CartModel
                    {
                        Product = objwebsiteBanHangEntities.Products.Find(id),
                        Quantity = quantity
                    });
                    // Cập nhật số lượng sản phẩm trong giỏ hàng
                    Session["count"] = Convert.ToInt32(Session["count"]) + 1;
                }
                Session["cart"] = cart;
            }
            return Json(new { Message = "Thành công", JsonRequestBehavior.AllowGet });
        }

        private int isExist(int id)
        {
            List<CartModel> cart = (List<CartModel>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Product.Id.Equals(id))
                    return i;
            return -1;
        }

        //xóa sản phẩm khỏi giỏ hàng theo id
        public ActionResult Remove(int Id)
        {
            List<CartModel> li = (List<CartModel>)Session["cart"];
            li.RemoveAll(x => x.Product.Id == Id);
            Session["cart"] = li;
            Session["count"] = Convert.ToInt32(Session["count"]) - 1;
            return Json(new { Message = "Thành công", JsonRequestBehavior.AllowGet });
        }
    }
}
