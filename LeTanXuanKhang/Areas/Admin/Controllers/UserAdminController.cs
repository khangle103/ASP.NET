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
    public class UserAdminController : Controller
    {
        // GET: Admin/UserAdmin
        WebsiteBanHangEntities4 objwebsiteBanHangEntities = new WebsiteBanHangEntities4();

        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var lstUser  = new List<User_New>();
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
                lstUser = objwebsiteBanHangEntities.User_New
                    .Where(n => n.FirstName.Contains(SearchString))
                    .ToList();
            }
            else
            {
                // Lấy tất cả sản phẩm trong bảng product  
                lstUser = objwebsiteBanHangEntities.User_New.ToList();
            }

            ViewBag.CurrentFilter = SearchString; // Lưu giá trị lọc hiện tại vào ViewBag  
            int pageSize = 4; // Số lượng item của 1 trang  
            int pageNumber = (page ?? 1); // Lấy số trang hiện tại, mặc định là 1  
            lstUser = lstUser.OrderByDescending(n => n.Id).ToList(); // Sắp xếp theo id, sản phẩm mới lên đầu  
            return View(lstUser.ToPagedList(pageNumber, pageSize)); // Trả về view với danh sách đã phân trang  
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(User_New objUser)
        {
            try
            {
                // Mã hóa mật khẩu
                if (!string.IsNullOrEmpty(objUser.Password))
                {
                    objUser.Password = HashPassword(objUser.Password);
                }

                // Thêm người dùng vào cơ sở dữ liệu
                objwebsiteBanHangEntities.User_New.Add(objUser);
                objwebsiteBanHangEntities.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }

        }

        // Hàm mã hóa mật khẩu
        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            var lstUser = objwebsiteBanHangEntities.User_New.Where(n => n.Id == id).FirstOrDefault();
            return View(lstUser);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var lstUser = objwebsiteBanHangEntities.User_New.Where(n => n.Id == id).FirstOrDefault();
            return View(lstUser);
        }
        [HttpPost]
        public ActionResult Delete(User_New objCa)
        {
            var lstUser = objwebsiteBanHangEntities.User_New.Where(n => n.Id == objCa.Id).FirstOrDefault();

            objwebsiteBanHangEntities.User_New.Remove(lstUser);
            objwebsiteBanHangEntities.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var lstUser = objwebsiteBanHangEntities.User_New.Where(n => n.Id == id).FirstOrDefault();
            return View(lstUser);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User_New objUser)
        {
            try
            {
                var existingUser = objwebsiteBanHangEntities.User_New.Find(objUser.Id);
                if (existingUser == null)
                {
                    return HttpNotFound();
                }

                // Cập nhật các trường khác
                existingUser.FirstName = objUser.FirstName;
                existingUser.LastName = objUser.LastName;
                existingUser.Email = objUser.Email;
                existingUser.Password = objUser.Password;

                // Đánh dấu thực thể là đã chỉnh sửa
                objwebsiteBanHangEntities.Entry(existingUser).State = EntityState.Modified;

                // Lưu thay đổi vào cơ sở dữ liệu
                objwebsiteBanHangEntities.SaveChanges();

                TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần thiết (sử dụng thư viện log như NLog hoặc Serilog)
                TempData["ErrorMessage"] = "Đã xảy ra lỗi trong quá trình cập nhật sản phẩm. Vui lòng thử lại.";
                return RedirectToAction("Edit", new { id = objUser.Id });
            }
        }
    }
}