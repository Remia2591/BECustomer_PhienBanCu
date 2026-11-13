using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeTaiLyThuyet.Models
{
    [MetadataType(typeof(DanhMucMetadata))]
    public partial class DanhMuc
    {
    }
    public class DanhMucMetadata
    {
        [Required(ErrorMessage = "Mã loại là bắt buộc.")]
        [StringLength(50, ErrorMessage = "Mã loại tối đa 50 ký tự.")]
        public string MaLoai { get; set; }

        [Required(ErrorMessage = "Tên danh mục là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên danh mục tối đa 100 ký tự.")]
        public string TenDanhMuc { get; set; }
    }
}