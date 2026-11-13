using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeTaiLyThuyet.Models;

namespace DeTaiLyThuyet.Areas.Admin.Controllers
{
    public class DonHangsController : Controller
    {
        private readonly db_ad03e2_giftshopdbEntities db =
            new db_ad03e2_giftshopdbEntities();

        // GET: Admin/DonHangs
        public ActionResult Index()
        {
            var donHangs = db.DonHangs
                .Include(d => d.TaiKhoan)
                .OrderByDescending(d => d.NgayDat)
                .ToList();

            return View(donHangs);
        }

        // GET: Admin/DonHangs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DonHang donHang = db.DonHangs
                .Include("TaiKhoan")
                .Include("DonHangChiTiets.SanPham")
                .SingleOrDefault(d => d.DonHangID == id.Value);

            if (donHang == null)
            {
                return HttpNotFound();
            }

            return View(donHang);
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