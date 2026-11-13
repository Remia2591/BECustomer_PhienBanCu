using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeTaiLyThuyet.Models;
using System.Data.Entity;

namespace DeTaiLyThuyet.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult DonHang()
        {
            if (Session["TaiKhoanID"] == null)
            {
                return RedirectToAction("DangNhap", "Account");
            }

            int taiKhoanID = Convert.ToInt32(Session["TaiKhoanID"]);

            using (var db = new db_ad03e2_giftshopdbEntities())
            {
                var donHangs = db.DonHangs
                    .Include("DonHangChiTiets")
                    .Where(d => d.TaiKhoanID == taiKhoanID)
                    .OrderByDescending(d => d.NgayDat)
                    .ToList();

                return View(donHangs);
            }
        }

        public ActionResult ThanhToan()
        {
            if (Session["TaiKhoanID"] == null)
            {
                return RedirectToAction("DangNhap", "Account");
            }

            return View();
        }
        public class GioHangDatHang
        {
            public int ma { get; set; }
            public int sl { get; set; }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult XuLyDatHang(
    string ten,
    string sdt,
    string diaChi,
    string ptt,
    string gioHangJson)
        {
            if (Session["TaiKhoanID"] == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Vui lòng đăng nhập để đặt hàng."
                });
            }

            ten = (ten ?? "").Trim();
            sdt = (sdt ?? "").Trim();
            diaChi = (diaChi ?? "").Trim();

            if (string.IsNullOrWhiteSpace(ten) ||
                string.IsNullOrWhiteSpace(sdt) ||
                string.IsNullOrWhiteSpace(diaChi))
            {
                return Json(new
                {
                    success = false,
                    message = "Vui lòng nhập đầy đủ thông tin giao hàng."
                });
            }

            if (ptt != "Thanh toán khi nhận hàng" &&
                ptt != "Chuyển khoản ngân hàng")
            {
                return Json(new
                {
                    success = false,
                    message = "Phương thức thanh toán không hợp lệ."
                });
            }

            List<GioHangDatHang> gioHang;

            try
            {
                gioHang = Newtonsoft.Json.JsonConvert
                    .DeserializeObject<List<GioHangDatHang>>(gioHangJson);
            }
            catch
            {
                return Json(new
                {
                    success = false,
                    message = "Dữ liệu giỏ hàng không hợp lệ."
                });
            }

            if (gioHang == null || gioHang.Count == 0 ||
                gioHang.Count > 100 ||
                gioHang.Any(x => x.ma <= 0 || x.sl <= 0 || x.sl > 1000) ||
                gioHang.Select(x => x.ma).Distinct().Count() != gioHang.Count)
            {
                return Json(new
                {
                    success = false,
                    message = "Giỏ hàng không hợp lệ."
                });
            }

            try
            {
                using (var db = new db_ad03e2_giftshopdbEntities())
                using (var transaction = db.Database.BeginTransaction(
                    System.Data.IsolationLevel.Serializable))
                {
                    var donHang = new DonHang
                    {
                        MaDonHang = "DH" + Guid.NewGuid()
                            .ToString("N").Substring(0, 10).ToUpper(),
                        TaiKhoanID = Convert.ToInt32(Session["TaiKhoanID"]),
                        NgayDat = DateTime.Now,
                        TenNguoiNhan = ten,
                        SoDienThoai = sdt,
                        DiaChiGiaoHang = diaChi,
                        PhuongThucThanhToan = ptt,
                        TrangThai = "Chờ xác nhận",
                        TongTien = 0
                    };

                    decimal tongTien = 0;

                    foreach (var item in gioHang.OrderBy(x => x.ma))
                    {
                        var sanPham = db.SanPhams
                            .FirstOrDefault(x => x.SanPhamID == item.ma);

                        if (sanPham == null || !sanPham.DangBan)
                        {
                            return Json(new
                            {
                                success = false,
                                message = "Có sản phẩm không còn được bán."
                            });
                        }

                        int tonKho = Convert.ToInt32(sanPham.SoLuongTon);
                        decimal donGia = Convert.ToDecimal(sanPham.Gia);

                        if (donGia <= 0 || tonKho < item.sl)
                        {
                            return Json(new
                            {
                                success = false,
                                message = "Sản phẩm " + sanPham.TenSanPham +
                                          " không đủ hàng hoặc giá không hợp lệ."
                            });
                        }

                        decimal thanhTien = donGia * item.sl;
                        tongTien += thanhTien;

                        donHang.DonHangChiTiets.Add(new DonHangChiTiet
                        {
                            SanPhamID = sanPham.SanPhamID,
                            TenSanPham = sanPham.TenSanPham,
                            DonGia = donGia,
                            SoLuong = item.sl,
                            ThanhTien = thanhTien
                        });

                        sanPham.SoLuongTon -= item.sl;
                    }

                    donHang.TongTien = tongTien;

                    db.DonHangs.Add(donHang);
                    db.SaveChanges();
                    transaction.Commit();

                    return Json(new
                    {
                        success = true,
                        message = "Đặt hàng thành công!",
                        maDonHang = donHang.MaDonHang,
                        redirectUrl = Url.Action("DonHang", "Order")
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());

                return Json(new
                {
                    success = false,
                    message = "Không thể lưu đơn hàng. Vui lòng kiểm tra dữ liệu hoặc thử lại."
                });
            }
        }
    }
}