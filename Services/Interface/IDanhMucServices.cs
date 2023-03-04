using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;

namespace WebTools.Services.Interface
{
    public interface IDanhMucServices
    {
        Task<List<DanhMuc>> Get_LoaiVanBanHienThi();
        Task<List<DanhMuc>> Get_LoaiCabinet();
        Task<List<DanhMuc_Depts>> Get_DM_Depts();
        Task<List<DanhMuc_DoiTuongApDung>> Get_DM_DoiTuongApDung();
        Task<List<DanhMuc_HRMUsers>> Get_DM_NguoiSoanThao();
        Task<List<DanhMuc_LoaiBieuMau>> Get_DM_LoaiBieuMau();
        Task<List<DanhMuc_NhomQuyen>> Get_DM_NhomQuyen();
        Task<List<DanhMuc_LoaiBieuMau>> Get_DM_TrangThaiVanBan();
        Task<List<DanhMuc_Cabinet>> Get_DM_Cabinet();
        Task<List<DanhMuc_QuyenLoaiBM>> Get_DM_QuyenLoaiVB();
    }
}
