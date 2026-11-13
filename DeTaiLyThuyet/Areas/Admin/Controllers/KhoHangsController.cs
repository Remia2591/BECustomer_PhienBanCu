using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using DeTaiLyThuyet.Models;
using DeTaiLyThuyet.Models.ViewModel;

namespace DeTaiLyThuyet.Areas.Admin.Controllers
{
    public class KhoHangsController : Controller
    {
        // GET: Admin/KhoHangs
        private readonly db_ad03e2_giftshopdbEntities db =
            new db_ad03e2_giftshopdbEntities();

        // GET: Admin/KhoHangs
        public ActionResult Index()
        {
            var model = new QuanLyKhoVM
            {
                HangTon = db.SanPhams
                    .Include(sp => sp.DanhMuc)
                    .Where(sp => sp.SoLuongTon > 0)
                    .OrderBy(sp => sp.TenSanPham)
                    .ToList(),

                HangCanNhap = db.SanPhams
                    .Include(sp => sp.DanhMuc)
                    .Where(sp => sp.SoLuongTon == 0)
                    .OrderBy(sp => sp.TenSanPham)
                    .ToList()
            };

            return View(model);
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