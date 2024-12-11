using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeTanXuanKhang.Context;
namespace LeTanXuanKhang.Controllers
{
    public class HomeController : Controller
    {
        WebsiteBanHangEntities objwebsiteBanHangEntities = new WebsiteBanHangEntities();
        public ActionResult Index()
        {
            var lstCategory = objwebsiteBanHangEntities.Category.ToList();
            return View(lstCategory);
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