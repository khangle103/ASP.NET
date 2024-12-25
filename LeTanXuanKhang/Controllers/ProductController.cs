using LeTanXuanKhang.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeTanXuanKhang.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        WebsiteBanHangEntities objwebsiteBanHangEntities = new WebsiteBanHangEntities();

        public ActionResult Detail(int id)
        {
            var objProduct = objwebsiteBanHangEntities.Product.Where(n => n.Id == id).FirstOrDefault();
            return View(objProduct);
        }
    }
}