using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeTaiLyThuyet.Models;

namespace DeTaiLyThuyet.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult GioHang()
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
                        mota = p.MoTa,
                        soLuongTon = p.SoLuongTon
                    })
                    .ToList();

                ViewBag.SanPhamDB = sanPhams;
            }

            return View();
        }
    }
}