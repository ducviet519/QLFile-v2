using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace WebTools.Models.Entities
{
    public class VanBan
    {

    }

    public class BangTinVanBan
    {
        public string IDVB { get; set; }
        public string TenVB { get; set; }
        public string MaVB { get; set; }
        public string FileLink { get; set; }
        public string NgayCapNhatReView { get; set; }
        public string PhienBan { get; set; }
        public string NgayPhatHanh { get; set; }
        public string NgayHieuLuc { get; set; }
        public string NgayKiemSoatLai { get; set; }
        public string BPSoanThao { get; set; }
        public string DVApDung { get; set; }
        public string DoiTuongApDung { get; set; }
    }

    public class BangTinVanBan_XemXetLai
    {
        public string IDVB { get; set; }
        public string TenVB { get; set; }
        public string MaVB { get; set; }
        public string NgayKiemSoatLai { get; set; }
    }
    public class PreviewVanBan
    {
        public string IDVB { get; set; }
        public string TenVB { get; set; }
        public string FileLink { get; set; }
        public int Readed { get; set; }
        public int Downloaded { get; set; }
        public int Printed { get; set; }
        public int YeuThich { get; set; }
        public int Watermark { get; set; }
    }

    public class Search_VanBanChiTiet
    {
        public string loaitimkiem { get; set; }
        public string TenVB { get; set; }
        public string LoaiVB { get; set; }
        public string NgayBHBD { get; set; }
        public string NgayBHKT { get; set; }
        public string NgayHLBD { get; set; }
        public string NgayHLKT { get; set; }
        public string BPSoanThao { get; set; }
        public string DonViApDung { get; set; }
        public string DoiTuongApDung { get; set; }
        public string user { get; set; }
    }

    public class VanBanChiTiet
    {
        public string Readed { get; set; }
        public string Checked { get; set; }
        public string IDVB { get; set; }
        public string TenVB { get; set; }
        public string MaVB { get; set; }
        public string PhienBan { get; set; }
        public string FileLinkVB { get; set; }
        public string NgayPhatHanh { get; set; }
        public string NgayHieuLuc { get; set; }
        public string NgayXemLaiKeTiep { get; set; }
        public string BPSoanThao { get; set; }
        public string TrangThai { get; set; }
        public string DoiTuongApDung { get; set; }
        public string DVApDung { get; set; }
        public string ViTriCabinet { get; set; }
        public string PhanMem { get; set; }
    }

    public class VanBan_PhienBan
    {
        public string ID { get; set; }
        public string TenVanBan { get; set; }
        public string MaVanBan { get; set; }
        public string DonViApDung { get; set; }
        public string DoiTuongApDung { get; set; }
        public string BPSoanThao { get; set; }
        public string ThongTinCNTT { get; set; }
        public string STT { get; set; }
        public string PhienBan { get; set; }
        public string NgayBanHanh { get; set; }
        public string NgayApDung { get; set; }
        public string NgayXLKeTiep { get; set; }
        public string NgayXL { get; set; }
        public string NguoiSoanThao { get; set; }
        public string TrangThai { get; set; }
        public string FileLink { get; set; }
        public string GhiChu { get; set; }
        public string NgayUpload { get; set; }
        public string IDPhienBan { get; set; }
        public string IDFileLink { get; set; }

        [DataType(DataType.Upload)]
        [BindProperty]
        public IFormFile fileUpload { get; set; }
    }

    public class VanBan_ID
    {
        public string ReportID { get; set; }
    }
    public class Cabinet
    {
        public int CabinetID { get; set; }
    }

    public class VanBan_DoiThuMuc
    {
        public List<VanBan_ID> listID { get; set; }
        public List<Cabinet> listCabinet { get; set; }
    }

    public class VanBan_ListName
    {
        public string ID { get; set; }
        public string TenVB { get; set; }
        public string MaVB { get; set; }
    }

    public class VanBan_CRUD
    {
        public string ID { get; set; }
        public int ParentID { get; set; }
        public string TenVanBan { get; set; }
        public string MaVanBan { get; set; }
        public int LoaiVanBan { get; set; }
        public int LoaiBieuMau { get; set; }
        public string NgayBanHanh { get; set; }
        public string NgayApDung { get; set; }
        public int PhienBan { get; set; }
        public string BPSoanThao { get; set; }
        public string NguoiSoanThao { get; set; }
        public string DonViApDung { get; set; }
        public string DoiTuongApDung { get; set; }
        public string FileLink { get; set; }
        public bool Watermark { get; set; }
        public string GhiChu { get; set; }
        public string listID { get; set; }

        [DataType(DataType.Upload)]
        [BindProperty]
        public IFormFile fileUpload { get; set; }
    }

    public class VanBan_ThemVM
    {
        public List<VanBan_ID> listID { get; set; }
        public List<VanBan_CRUD> listReport { get; set; }
    }

    public class VanBan_ByID
    {
        public string ID { get; set; }
        public string TenVanBan { get; set; }
        public string MaVanBan { get; set; }
        public int TheLoai { get; set; }
        public int IDCabinet { get; set; }
        public string DonViApDungID { get; set; }
        public string DoiTuongApDungID { get; set; }
        public int Watermark { get; set; }
        public string id { get; set; }
        public string TenVB { get; set; }
        public string MaVB { get; set; }
    }

    public class VanBan_BanHanh
    {
        public string TenVanBan { get; set; }
        public string MaVanBan { get; set; }
        public string NoiDung { get; set; }
        public string KPApDung { get; set; }
        public string DoiTuongApDung { get; set; }
        public string NgayBanHanh { get; set; }
        public string NgayHieuLuc { get; set; }
        public string ID { get; set; }
        public string IDPhienBan { get; set; }
    }

    public class VanBanPhienBan_ID
    {
        public string IDVanBan { get; set; }
        public string IDPhienBan { get; set; }
    }

    public class VanBanBanHanhRequest
    {
        public string emailBody { get; set; }
        public List<string> listEmail { get; set; }
        public List<VanBanPhienBan_ID> listID { get; set; }
    }
}
