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
        public ActionResult Grid()
        {
            return View();
        }
        public ActionResult Large()
        {
            return View();
        }
    }
}