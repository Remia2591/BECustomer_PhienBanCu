using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using DeTaiLyThuyet.Models;

namespace DeTaiLyThuyet.Areas.Admin.Controllers
{
    public class TaiKhoansController : Controller
    {
        private readonly db_ad03e2_giftshopdbEntities db =
           new db_ad03e2_giftshopdbEntities();

        // GET: Admin/TaiKhoans
        public ActionResult Index()
        {
            // Quy ước VaiTro của khách hàng là "KhachHang".
            var khachHangs = db.TaiKhoans
                .Where(t => t.VaiTro == "KhachHang")
                .OrderBy(t => t.HoTen)
                .ToList();

            return View(khachHangs);
        }

        // GET: Admin/TaiKhoans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TaiKhoan taiKhoan = db.TaiKhoans
                .Include(t => t.DonHangs)
                .SingleOrDefault(t =>
                    t.TaiKhoanID == id.Value &&
                    t.VaiTro == "KhachHang");

            if (taiKhoan == null)
            {
                return HttpNotFound();
            }

            return View(taiKhoan);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}