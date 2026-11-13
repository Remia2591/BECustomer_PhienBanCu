using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeTaiLyThuyet.Models;

namespace DeTaiLyThuyet.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var db = new db_ad03e2_giftshopdbEntities())
            {
                var sanPhams = db.SanPhams
                    .Where(p => p.DangBan)
                    .Select(p => new
                    {
                        ma = p.SanPhamID,
                        ten = p.TenSanPham,
                        hinh = p.HinhAnh,
                        gia = p.Gia,
                        loai = p.DanhMuc.MaLoai,
                        mota = p.MoTa
                    })
                    .ToList();

                ViewBag.SanPhamDB = sanPhams;
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Combo()
        {
            return View();
        }

        public ActionResult LienHe()
        {
            return View();
        }
    }
}