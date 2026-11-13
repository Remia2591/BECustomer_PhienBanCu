using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeTaiLyThuyet.Models.ViewModel
{
    public class DanhMucIndexVM
    {
        public int DanhMucID { get; set; }
        public string MaLoai { get; set; }
        public string TenDanhMuc { get; set; }
        public int SoLuongSanPham { get; set; }
    }
}