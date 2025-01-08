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
        WebsiteBanHangEntities4 objwebsiteBanHangEntities = new WebsiteBanHangEntities4();

        // GET: Category
        public ActionResult AllCategory()
        {
            var lstCategory = objwebsiteBanHangEntities.Category.ToList();
            return View(lstCategory);
        }
    }
}