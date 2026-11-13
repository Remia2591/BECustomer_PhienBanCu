using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DeTaiLyThuyet.Models
{
    [MetadataType(typeof(SanPhamMetadata))]
    public partial class SanPham
    {
    }

    public class SanPhamMetadata
    {
        [Display(Name = "Danh mục")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn danh mục.")]
        public int DanhMucID { get; set; }

        [Display(Name = "Tên sản phẩm")]
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
        [StringLength(200, ErrorMessage = "Tên sản phẩm tối đa 200 ký tự.")]
        public string TenSanPham { get; set; }

        [Display(Name = "Đường dẫn ảnh")]
        [StringLength(255, ErrorMessage = "Đường dẫn ảnh tối đa 255 ký tự.")]
        public string HinhAnh { get; set; }

        [Display(Name = "Giá")]
        [Range(0.01, 999999999,
            ErrorMessage = "Giá phải lớn hơn 0 và không vượt quá 999.999.999.")]
        public decimal Gia { get; set; }

        [Display(Name = "Mô tả")]
        [StringLength(2000, ErrorMessage = "Mô tả tối đa 2000 ký tự.")]
        public string MoTa { get; set; }

        [Display(Name = "Số lượng tồn")]
        [Range(0, int.MaxValue,
            ErrorMessage = "Số lượng tồn phải lớn hơn hoặc bằng 0.")]
        public int SoLuongTon { get; set; }

        [Display(Name = "Đang bán")]
        public bool DangBan { get; set; }
    }
}