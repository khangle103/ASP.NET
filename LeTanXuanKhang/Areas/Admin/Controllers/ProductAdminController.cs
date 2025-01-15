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
    public class ProductAdminController : Controller
    {
        // GET: Admin/ProductAdmin
        WebsiteBanHangEntities4 objwebsiteBanHangEntities = new WebsiteBanHangEntities4();

        public ActionResult ProductList(string currentFilter, string SearchString, int? page)
        {
            var lstProduct = new List<Product>();
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
                lstProduct = objwebsiteBanHangEntities.Products
                    .Where(n => n.Name.Contains(SearchString))
                    .ToList();
            }
            else
            {
                // Lấy tất cả sản phẩm trong bảng product  
                lstProduct = objwebsiteBanHangEntities.Products.ToList();
            }

            ViewBag.CurrentFilter = SearchString; // Lưu giá trị lọc hiện tại vào ViewBag  
            int pageSize = 4; // Số lượng item của 1 trang  
            int pageNumber = (page ?? 1); // Lấy số trang hiện tại, mặc định là 1  
            lstProduct = lstProduct.OrderByDescending(n => n.Id).ToList(); // Sắp xếp theo id, sản phẩm mới lên đầu  
            return View(lstProduct.ToPagedList(pageNumber, pageSize)); // Trả về view với danh sách đã phân trang  
        }
        
        [HttpGet]
        public ActionResult Create()
        {
            Common objCommon = new Common();
            // Lấy dữ liệu danh mục dưới DB  
            var lstCat = objwebsiteBanHangEntities.Categories.ToList();
            // Convert sang select list dạng value, text  
            ListToDataTableConverter converter = new ListToDataTableConverter();
            DataTable dtCategory = converter.ToDataTable(lstCat);
            ViewBag.ListCategory = objCommon.ToSelectList(dtCategory, "Id", "Name");

            // Lấy dữ liệu thương hiệu dưới DB  
            var lstBrand = objwebsiteBanHangEntities.Brands.ToList();
            DataTable dtBrand = converter.ToDataTable(lstBrand);
            // Convert sang select list dạng value, text  
            ViewBag.ListBrand = objCommon.ToSelectList(dtBrand, "Id", "Name");

            // Loại sản phẩm  
            List<ProductType> lstProductType = new List<ProductType>();

            ProductType objProductType = new ProductType();
            objProductType.Id = 01;
            objProductType.Name = "Giảm giá sốc";
            lstProductType.Add(objProductType);

            objProductType = new ProductType(); // Tạo một đối tượng mới  
            objProductType.Id = 02;
            objProductType.Name = "Đề xuất";
            lstProductType.Add(objProductType);

            DataTable dtProductType = converter.ToDataTable(lstProductType);
            // Convert sang select list dạng value, text  
            ViewBag.ProductType = objCommon.ToSelectList(dtProductType, "Id", "Name");
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(Product objProduct)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (objProduct.ImageUpload != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpload.FileName);
                        string extension = Path.GetExtension(objProduct.ImageUpload.FileName);
                        fileName = fileName + extension;

                        objProduct.Avatar = fileName;
                        objProduct.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/"), fileName));
                    }

                    objProduct.CreatedOnUtc = DateTime.Now;
                    objwebsiteBanHangEntities.Products.Add(objProduct);
                    objwebsiteBanHangEntities.SaveChanges();

                    return RedirectToAction("ProductList");
                }
                catch
                {
                    // Khởi tạo lại ViewBag nếu có lỗi
                    Create();
                    return View(objProduct);
                }
            }

            // Khởi tạo lại ViewBag nếu validation thất bại
            Create();
            return View(objProduct);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var objProduct = objwebsiteBanHangEntities.Products.Where(n => n.Id == id).FirstOrDefault();
            return View(objProduct);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objProduct = objwebsiteBanHangEntities.Products.Where(n => n.Id == id).FirstOrDefault();
            return View(objProduct);
        }
        [HttpPost]
        public ActionResult Delete(Product objPro)
        {
            var objProduct = objwebsiteBanHangEntities.Products.Where(n => n.Id == objPro.Id).FirstOrDefault();

            objwebsiteBanHangEntities.Products.Remove(objProduct);
            objwebsiteBanHangEntities.SaveChanges();

            return RedirectToAction("ProductList");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objProduct = objwebsiteBanHangEntities.Products.Where(n => n.Id == id).FirstOrDefault();
            return View(objProduct);
        }
        [HttpPost]
        public ActionResult Edit(Product objProduct)
        {
            if (objProduct.ImageUpload != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpload.FileName);
                string extension = Path.GetExtension(objProduct.ImageUpload.FileName);
                fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                objProduct.Avatar = fileName;
                objProduct.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/"), fileName));
            }

            objwebsiteBanHangEntities.Entry(objProduct).State = EntityState.Modified;
            objwebsiteBanHangEntities.SaveChanges();

            return RedirectToAction("ProductList");
        }
    }
}