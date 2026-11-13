using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeTaiLyThuyet.Models;
using System.Web.Helpers;

namespace DeTaiLyThuyet.Controllers
{
    public class AccountController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKy(string hoTen, string email, string matKhau)
        {
            hoTen = (hoTen ?? "").Trim();
            email = (email ?? "").Trim().ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(hoTen) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(matKhau))
            {
                return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin." });
            }

            if (matKhau.Length < 6)
            {
                return Json(new { success = false, message = "Mật khẩu phải có ít nhất 6 ký tự." });
            }

            using (var db = new db_ad03e2_giftshopdbEntities())
            {
                if (db.TaiKhoans.Any(t => t.Email == email))
                {
                    return Json(new { success = false, message = "Email đã được đăng ký." });
                }

                var taiKhoan = new TaiKhoan
                {
                    HoTen = hoTen,
                    Email = email,
                    MatKhauHash = Crypto.HashPassword(matKhau),
                    VaiTro = "KhachHang",
                    NgayTao = DateTime.Now,
                    DangHoatDong = true
                };

                db.TaiKhoans.Add(taiKhoan);
                db.SaveChanges();
            }

            return Json(new { success = true, message = "Đăng ký thành công!" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult XuLyDangNhap(string email, string matKhau)
        {
            email = (email ?? "").Trim().ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(matKhau))
            {
                return Json(new
                {
                    success = false,
                    message = "Vui lòng nhập email và mật khẩu."
                });
            }

            using (var db = new db_ad03e2_giftshopdbEntities())
            {
                var taiKhoan = db.TaiKhoans.FirstOrDefault(t =>
                    t.Email == email && t.DangHoatDong);

                if (taiKhoan == null ||
                    !Crypto.VerifyHashedPassword(taiKhoan.MatKhauHash, matKhau))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Email hoặc mật khẩu không đúng."
                    });
                }

                Session["TaiKhoanID"] = taiKhoan.TaiKhoanID;
                Session["HoTen"] = taiKhoan.HoTen;
                Session["Email"] = taiKhoan.Email;
                Session["VaiTro"] = taiKhoan.VaiTro;

                return Json(new
                {
                    success = true,
                    message = "Đăng nhập thành công!",
                    redirectUrl = Url.Action("Index", "Home")
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangXuat()
        {
            Session.Clear();
            Session.Abandon();

            return Json(new
            {
                success = true,
                redirectUrl = Url.Action("Index", "Home")
            });
        }

        [HttpGet]
        public ActionResult TrangThai()
        {
            bool daDangNhap = Session["TaiKhoanID"] != null;

            string tokenDangXuat = daDangNhap
                ? System.Web.Helpers.AntiForgery.GetHtml().ToHtmlString()
                : "";

            return Json(new
            {
                daDangNhap = daDangNhap,
                hoTen = daDangNhap
                    ? (Session["HoTen"] as string ?? "")
                    : "",
                tokenDangXuat = tokenDangXuat
            }, JsonRequestBehavior.AllowGet);
        }
        // GET: Account
        public ActionResult DangNhap()
        {
            return View();
        }
    }
}