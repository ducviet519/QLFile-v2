using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models.Entities
{
    public class DanhMuc
    {
        public string ID { get; set; }
        public string DES { get; set; }
    }

    public class DanhMuc_LoaiVanBan
    {
        public string LoaiVB { get; set; }
        public string PVApDung { get; set; }
        public string Quyen { get; set; }
    }

    public class DanhMuc_Depts
    {
        public string STT { get; set; }
        public string Code { get; set; }
        public string KhoaP { get; set; }
        public string Status { get; set; }
        public string MaKPHCNS { get; set; }
        public string KhoaPKD { get; set; }
    }

    public class DanhMuc_DoiTuongApDung
    {
        public string ID { get; set; }
        public string TypeDes { get; set; }
    }

    public class DanhMuc_HRMUsers
    {
        public string ID { get; set; }
        public string UserName { get; set; }
        public string HoTen { get; set; }
        public string MaNV { get; set; }
    }

    public class DanhMuc_LoaiBieuMau
    {
        public string ID { get; set; }
        public string MoTa { get; set; }
    }

    public class DanhMuc_NhomQuyen
    {
        public string IDGroup { get; set; }
        public string Groupname { get; set; }
    }

    public class DanhMuc_Cabinet
    {
        public string NoiDung { get; set; }
        public string IDCabin { get; set; }
        public string TenCabin { get; set; }
    }

    public class DanhMuc_QuyenLoaiBM
    {
        public string ID { get; set; }
        public string TenLoaiBM { get; set; }
        public string Doc { get; set; }
        public string InRa { get; set; }
        public string TaiVe { get; set; }
        public string TrangThai { get; set; }
    }

    public class DanhMuc_KhoaPhong
    {
        public string ID { get; set; }
        public string KhoaP { get; set; }
    }
}
