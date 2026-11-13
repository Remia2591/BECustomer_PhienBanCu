using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeTaiLyThuyet.Models;
using DeTaiLyThuyet.Models.ViewModel;

namespace DeTaiLyThuyet.Areas.Admin.Controllers
{
    public class DanhMucsController : Controller
    {
        private db_ad03e2_giftshopdbEntities db = new db_ad03e2_giftshopdbEntities();

        // GET: Admin/DanhMucs
        public ActionResult Index()
        {
            var danhMucs = db.DanhMucs
               .Select(d => new DanhMucIndexVM
               {
                   DanhMucID = d.DanhMucID,
                   MaLoai = d.MaLoai,
                   TenDanhMuc = d.TenDanhMuc,
                   SoLuongSanPham = d.SanPhams.Count()
               })
               .ToList();

            return View(danhMucs);
        }

        // GET: Admin/DanhMucs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DanhMuc danhMuc = db.DanhMucs
                .Include(d => d.SanPhams)
                .SingleOrDefault(d => d.DanhMucID == id.Value);

            if (danhMuc == null)
            {
                return HttpNotFound();
            }
            return View(danhMuc);
        }

        // GET: Admin/DanhMucs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/DanhMucs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DanhMucID,MaLoai,TenDanhMuc")] DanhMuc danhMuc)
        {
            if (!ModelState.IsValid)
            {
                return View(danhMuc);
            }

            db.DanhMucs.Add(danhMuc);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Admin/DanhMucs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMuc danhMuc = db.DanhMucs.Find(id);
            if (danhMuc == null)
            {
                return HttpNotFound();
            }
            return View(danhMuc);
        }

        // POST: Admin/DanhMucs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DanhMucID,MaLoai,TenDanhMuc")] DanhMuc danhMuc)
        {
            if (!ModelState.IsValid)
            {
                return View(danhMuc);
            }
            DanhMuc danhMucTrongDb = db.DanhMucs.Find(danhMuc.DanhMucID);

            if (danhMucTrongDb == null)
            {
                return HttpNotFound();
            }

            danhMucTrongDb.MaLoai = danhMuc.MaLoai;
            danhMucTrongDb.TenDanhMuc = danhMuc.TenDanhMuc;

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Admin/DanhMucs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMuc danhMuc = db.DanhMucs.Find(id);
            if (danhMuc == null)
            {
                return HttpNotFound();
            }
            return View(danhMuc);
        }

        // POST: Admin/DanhMucs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DanhMuc danhMuc = db.DanhMucs.Find(id);
            if (danhMuc == null)
            {
                return HttpNotFound();
            }
            bool conSanPham = db.SanPhams.Any(sp => sp.DanhMucID == id);
            if (conSanPham)
            {
                return Content(
                    "Không thể xóa danh mục vì vẫn còn sản phẩm thuộc danh mục này.");
            }

            db.DanhMucs.Remove(danhMuc);
            db.SaveChanges();

            return RedirectToAction("Index");
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
