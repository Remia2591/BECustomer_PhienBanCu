using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeTaiLyThuyet.Models;

namespace DeTaiLyThuyet.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult ChiTiet(int? ma)
        {
            if (!ma.HasValue)
            {
                return HttpNotFound();
            }

            using (var db = new db_ad03e2_giftshopdbEntities())
            {
                var sanPham = db.SanPhams
                    .Include("DanhMuc")
                    .FirstOrDefault(p => p.SanPhamID == ma.Value);

                if (sanPham == null)
                {
                    return HttpNotFound();
                }

                return View(sanPham);
            }
        }

        public ActionResult SanPham()
        {
            return View();
        }
    }
}