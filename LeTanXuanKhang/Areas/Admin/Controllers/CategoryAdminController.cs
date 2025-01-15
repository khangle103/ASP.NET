using LeTanXuanKhang.Context;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static LeTanXuanKhang.Common;

namespace LeTanXuanKhang.Areas.Admin.Controllers
{
    public class CategoryAdminController : Controller
    {
        WebsiteBanHangEntities4 objwebsiteBanHangEntities = new WebsiteBanHangEntities4();

        // GET: Admin/CategoryAdmin
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var lstCategory = new List<Category>();
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }

            if (!string.IsNullOrEmpty(SearchString))
            {
                // Lấy sản phẩm theo từ khóa tìm kiếm  
                lstCategory = objwebsiteBanHangEntities.Categories
                    .Where(n => n.Name.Contains(SearchString))
                    .ToList();
            }
            else
            {
                // Lấy tất cả sản phẩm trong bảng product  
                lstCategory = objwebsiteBanHangEntities.Categories.ToList();
            }

            ViewBag.CurrentFilter = SearchString; // Lưu giá trị lọc hiện tại vào ViewBag  
            int pageSize = 4; // Số lượng item của 1 trang  
            int pageNumber = (page ?? 1); // Lấy số trang hiện tại, mặc định là 1  
            lstCategory = lstCategory.OrderByDescending(n => n.Id).ToList(); // Sắp xếp theo id, sản phẩm mới lên đầu  
            return View(lstCategory.ToPagedList(pageNumber, pageSize)); // Trả về view với danh sách đã phân trang  
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category objCategory)
        {
            try
            {
                if (objCategory.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objCategory.ImageUpload.FileName);
                    string extension = Path.GetExtension(objCategory.ImageUpload.FileName);
                    fileName = fileName + extension;
                    objCategory.Avatar = fileName;
                    objCategory.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items/"), fileName));
                }

                objwebsiteBanHangEntities.Categories.Add(objCategory);
                objwebsiteBanHangEntities.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            var objCategory = objwebsiteBanHangEntities.Categories.Where(n => n.Id == id).FirstOrDefault();
            return View(objCategory);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objCategory = objwebsiteBanHangEntities.Categories.Where(n => n.Id == id).FirstOrDefault();
            return View(objCategory);
        }
        [HttpPost]
        public ActionResult Delete(Category objCa)
        {
            var objCategory = objwebsiteBanHangEntities.Categories.Where(n => n.Id == objCa.Id).FirstOrDefault();

            objwebsiteBanHangEntities.Categories.Remove(objCategory);
            objwebsiteBanHangEntities.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objCategory = objwebsiteBanHangEntities.Categories.Where(n => n.Id == id).FirstOrDefault();
            return View(objCategory);
        }
        [HttpPost]
        public ActionResult Edit(Category objCategory)
        {
            if (objCategory.ImageUpload != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objCategory.ImageUpload.FileName);
                string extension = Path.GetExtension(objCategory.ImageUpload.FileName);
                fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                objCategory.Avatar = fileName;
                objCategory.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/"), fileName));
            }

            objwebsiteBanHangEntities.Entry(objCategory).State = EntityState.Modified;
            objwebsiteBanHangEntities.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
