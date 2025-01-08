using LeTanXuanKhang.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeTanXuanKhang.Areas.Admin.Controllers
{
    public class ProductAdminController : Controller
    {
        // GET: Admin/ProductAdmin
        WebsiteBanHangEntities4 objwebsiteBanHangEntities = new WebsiteBanHangEntities4();

        public ActionResult ProductList()
        {
            return View();
        }
    }
}