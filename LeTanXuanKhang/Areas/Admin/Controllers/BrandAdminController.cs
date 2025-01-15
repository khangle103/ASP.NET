
using LeTanXuanKhang.Context;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeTanXuanKhang.Areas.Admin.Controllers
{
    public class BrandAdminController : Controller
    {
        WebsiteBanHangEntities4 objwebsiteBanHangEntities = new WebsiteBanHangEntities4();

        // GET: Admin/CategoryAdmin
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var lstBrand = new List<Brand>();
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
                lstBrand = objwebsiteBanHangEntities.Brands
                    .Where(n => n.Name.Contains(SearchString))
                    .ToList();
            }
            else
            {
                // Lấy tất cả sản phẩm trong bảng product  
                lstBrand = objwebsiteBanHangEntities.Brands.ToList();
            }

            ViewBag.CurrentFilter = SearchString; // Lưu giá trị lọc hiện tại vào ViewBag  
            int pageSize = 4; // Số lượng item của 1 trang  
            int pageNumber = (page ?? 1); // Lấy số trang hiện tại, mặc định là 1  
            lstBrand = lstBrand.OrderByDescending(n => n.Id).ToList(); // Sắp xếp theo id, sản phẩm mới lên đầu  
            return View(lstBrand.ToPagedList(pageNumber, pageSize)); // Trả về view với danh sách đã phân trang  
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Brand objBrand)
        {
            try
            {
                if (objBrand.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objBrand.ImageUpload.FileName);
                    string extension = Path.GetExtension(objBrand.ImageUpload.FileName);
                    fileName = fileName + extension;
                    objBrand.Avatar = fileName;
                    objBrand.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items/"), fileName));
                }

                objwebsiteBanHangEntities.Brands.Add(objBrand);
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
            var objBrand = objwebsiteBanHangEntities.Brands.Where(n => n.Id == id).FirstOrDefault();
            return View(objBrand);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objBrand = objwebsiteBanHangEntities.Brands.Where(n => n.Id == id).FirstOrDefault();
            return View(objBrand);
        }
        [HttpPost]
        public ActionResult Delete(Brand objCa)
        {
            var objBrand = objwebsiteBanHangEntities.Brands.Where(n => n.Id == objCa.Id).FirstOrDefault();

            objwebsiteBanHangEntities.Brands.Remove(objBrand);
            objwebsiteBanHangEntities.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objBrand = objwebsiteBanHangEntities.Brands.Where(n => n.Id == id).FirstOrDefault();
            return View(objBrand);
        }
        [HttpPost]
        public ActionResult Edit(Brand objBrand)
        {
            if (objBrand.ImageUpload != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objBrand.ImageUpload.FileName);
                string extension = Path.GetExtension(objBrand.ImageUpload.FileName);
                fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                objBrand.Avatar = fileName;
                objBrand.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/"), fileName));
            }

            objwebsiteBanHangEntities.Entry(objBrand).State = EntityState.Modified;
            objwebsiteBanHangEntities.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}