using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models.Entities
{
    public class ThongKe
    {
    }

    public class ThongKe_BieuDo
    {
        public string TenLoai { get; set; }
        public string SoLuong { get; set; }
        public string TenBC { get; set; }
        public string LoaiBC { get; set; }
    }

    public class ThongKe_BaoCaoDocHieu
    {
        public string TenVanBan { get; set; }
        public string MaVanBan { get; set; }
        public string SoLuongDoc { get; set; }
        public string SoLuongHieu { get; set; }
    }

    public class SearchThongKe
    {
        public string NgayBHBD { get; set; }
        public string NgayBHKT { get; set; }
        public string NgayXNBD { get; set; }
        public string NgayXNKT { get; set; }
        public string NgayDocBD { get; set; }
        public string NgayDocKT { get; set; }
        public string DoiTuong { get; set; }
        public string DonVi { get; set; }
        public string LoaiVB { get; set; }
        public string PhamVi { get; set; }
    }

    public class ThongKe_PhamViVB
    {
        public string DocID { get; set; }
        public string TenVanBan { get; set; }
        public string MaVanBan { get; set; }
        public string PhienBan { get; set; }
        public string Cabinet { get; set; }
        public string NgayBanHanh { get; set; }
        public string NgayHieuLuc { get; set; }
        public string NgayXLKT { get; set; }
        public string TrangThai { get; set; }
        public string GhiChu { get; set; }
    }
}
