using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeTaiLyThuyet.Models;

namespace DeTaiLyThuyet.Areas.Admin.Controllers
{
    public class SanPhamsController : Controller
    {
        private readonly db_ad03e2_giftshopdbEntities db =
            new db_ad03e2_giftshopdbEntities();

        // GET: Admin/SanPhams
        public ActionResult Index()
        {
            var sanPhams = db.SanPhams
                .Include(s => s.DanhMuc)
                .ToList();

            return View(sanPhams);
        }

        // GET: Admin/SanPhams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SanPham sanPham = db.SanPhams
                .Include(s => s.DanhMuc)
                .SingleOrDefault(s => s.SanPhamID == id.Value);

            if (sanPham == null)
            {
                return HttpNotFound();
            }

            return View(sanPham);
        }

        // GET: Admin/SanPhams/Create
        public ActionResult Create()
        {
            NapDanhMuc();
            return View();
        }

        // POST: Admin/SanPhams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
    [Bind(Include = "DanhMucID,TenSanPham,Gia,MoTa,SoLuongTon,DangBan")]
    SanPham sanPham,
    HttpPostedFileBase fileAnh)
        {
            try
            {
                if (fileAnh != null && fileAnh.ContentLength > 0)
                {
                    string duongDanAnh = LuuAnh(fileAnh);

                    if (duongDanAnh == null)
                    {
                        ModelState.AddModelError(
                            "fileAnh",
                            "Ảnh không hợp lệ hoặc vượt quá 5 MB.");
                    }
                    else
                    {
                        sanPham.HinhAnh = duongDanAnh;
                    }
                }

                if (!ModelState.IsValid)
                {
                    NapDanhMuc(sanPham.DanhMucID);
                    return View(sanPham);
                }

                db.SanPhams.Add(sanPham);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());

                ModelState.AddModelError(
                    "",
                    "Lỗi khi lưu sản phẩm: " + ex.GetBaseException().Message);

                NapDanhMuc(sanPham.DanhMucID);
                return View(sanPham);
            }
        }

        // GET: Admin/SanPhams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SanPham sanPham = db.SanPhams.Find(id);

            if (sanPham == null)
            {
                return HttpNotFound();
            }

            NapDanhMuc(sanPham.DanhMucID);
            return View(sanPham);
        }

        // POST: Admin/SanPhams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "SanPhamID,DanhMucID,TenSanPham,Gia,MoTa,SoLuongTon,DangBan")]
            SanPham sanPhamForm,
            HttpPostedFileBase fileAnh)
        {
            SanPham sanPham = db.SanPhams.Find(sanPhamForm.SanPhamID);

            if (sanPham == null)
            {
                return HttpNotFound();
            }

            if (fileAnh != null && fileAnh.ContentLength > 0)
            {
                string duongDanAnh = LuuAnh(fileAnh);

                if (duongDanAnh == null)
                {
                    ModelState.AddModelError(
                        "fileAnh",
                        "Ảnh phải có định dạng JPG, JPEG, PNG hoặc GIF và không quá 5 MB.");
                }
                else
                {
                    sanPham.HinhAnh = duongDanAnh;
                }
            }

            if (!ModelState.IsValid)
            {
                NapDanhMuc(sanPhamForm.DanhMucID);
                return View(sanPhamForm);
            }

            sanPham.DanhMucID = sanPhamForm.DanhMucID;
            sanPham.TenSanPham = sanPhamForm.TenSanPham;
            sanPham.Gia = sanPhamForm.Gia;
            sanPham.MoTa = sanPhamForm.MoTa;
            sanPham.SoLuongTon = sanPhamForm.SoLuongTon;
            sanPham.DangBan = sanPhamForm.DangBan;

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Admin/SanPhams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SanPham sanPham = db.SanPhams
                .Include(s => s.DanhMuc)
                .SingleOrDefault(s => s.SanPhamID == id.Value);

            if (sanPham == null)
            {
                return HttpNotFound();
            }

            return View(sanPham);
        }

        // POST: Admin/SanPhams/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPhams.Find(id);

            if (sanPham == null)
            {
                return HttpNotFound();
            }

            bool coTrongDonHang =
                db.DonHangChiTiets.Any(x => x.SanPhamID == id);

            bool coTrongGioHang =
                db.GioHangChiTiets.Any(x => x.SanPhamID == id);

            if (coTrongDonHang || coTrongGioHang)
            {
                return Content(
                    "Không thể xóa sản phẩm vì sản phẩm đang có trong đơn hàng hoặc giỏ hàng.");
            }

            db.SanPhams.Remove(sanPham);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // Nạp danh sách danh mục vào dropdown.
        private void NapDanhMuc(object danhMucDangChon = null)
        {
            ViewBag.DanhMucID = new SelectList(
                db.DanhMucs.OrderBy(d => d.TenDanhMuc),
                "DanhMucID",
                "TenDanhMuc",
                danhMucDangChon
            );
        }

        private string LuuAnh(HttpPostedFileBase fileAnh)
        {
            const int kichThuocToiDa = 5 * 1024 * 1024;

            if (fileAnh == null || fileAnh.ContentLength <= 0)
            {
                return null;
            }

            if (fileAnh.ContentLength > kichThuocToiDa)
            {
                return null;
            }

            string phanMoRong = Path.GetExtension(fileAnh.FileName);

            if (string.IsNullOrEmpty(phanMoRong))
            {
                return null;
            }

            phanMoRong = phanMoRong.ToLowerInvariant();

            string[] dinhDangHopLe = { ".jpg", ".jpeg", ".png", ".gif" };

            if (!dinhDangHopLe.Contains(phanMoRong))
            {
                return null;
            }

            string thuMuc = Server.MapPath("~/Content/images");

            if (!Directory.Exists(thuMuc))
            {
                Directory.CreateDirectory(thuMuc);
            }

            string tenFile = Guid.NewGuid().ToString("N") + phanMoRong;
            string duongDanDayDu = Path.Combine(thuMuc, tenFile);

            fileAnh.SaveAs(duongDanDayDu);

            return "/Content/images/" + tenFile;
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