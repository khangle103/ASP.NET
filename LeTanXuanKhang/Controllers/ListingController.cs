using LeTanXuanKhang.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeTanXuanKhang.Controllers
{
    public class ListingController : Controller
    {
        // GET: Listing
        WebsiteBanHangEntities4 objwebsiteBanHangEntities = new WebsiteBanHangEntities4();

        public ActionResult Grid()
        {
            return View();
        }
        public ActionResult Large(int id)
        {
            var lstLarge = objwebsiteBanHangEntities.Products.Where(n => n.CategoryId == id).ToList();

            return View(lstLarge);
        }
    }
}