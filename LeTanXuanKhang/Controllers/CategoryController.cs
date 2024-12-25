using LeTanXuanKhang.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeTanXuanKhang.Controllers
{
    public class CategoryController : Controller
    {
        WebsiteBanHangEntities objwebsiteBanHangEntities = new WebsiteBanHangEntities();

        // GET: Category
        public ActionResult AllCategory()
        {
            var lstCategory = objwebsiteBanHangEntities.Category.ToList();
            return View(lstCategory);
        }
    }
}