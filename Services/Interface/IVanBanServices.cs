using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;

namespace WebTools.Services.Interface
{
    public interface IVanBanServices
    {
        Task<List<BangTinVanBan>> Get_BangTinVanBan(string user, string loaivb = null);
        Task<List<PreviewVanBan>> Preview_VanBan(string user, string idvanban = null);
        Task<List<VanBanChiTiet>> Table_VanBanChiTiet(Search_VanBanChiTiet search = null);
        Task<List<VanBanChiTiet>> Table_VanBanChiTiet_Google(Search_VanBanChiTiet search = null, List<GoogleDriveFile> table = null);
        Task<string> ViewLog(string idvb, string user);
        Task<string> SendLike(string idvb, string user);
        Task<List<VanBan_PhienBan>> Get_DanhSachPhienBan(string idvb);
        Task<string> Upsert_PhienBan(VanBan_PhienBan phienban, string user);
        Task<string> StatUpdate_PhienBan(string idvb, string idphienban, int cmdtype, string user);
        Task<string> Upsert_VanBan_ThuMuc(List<VanBan_ID> listID, int IDCabinet, string user);
        Task<List<VanBan_ListName>> Get_ListTenVanBan(string text = null);
        Task<List<VanBan_ByID>> Get_VanBanByID(string docID = null);
        Task<string> Add_VanBan(VanBan_CRUD vanban, List<VanBan_ID> listID, string user);
        Task<string> Update_VanBan(VanBan_CRUD vanban, List<VanBan_ID> listID, string user);
        Task<string> Delete_VanBan(string idvb);
        Task<List<VanBan_BanHanh>> BanHanhVanBan(List<VanBan_ID> listID);
        Task<string> BanHanhVanBan_Upsert(List<VanBanPhienBan_ID> listID, string user);
        
    }
}
