using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeTaiLyThuyet.Models.ViewModel
{
    public class QuanLyKhoVM
    {
        public IEnumerable<SanPham> HangTon { get; set; }
        public IEnumerable<SanPham> HangCanNhap { get; set; }
    }
}