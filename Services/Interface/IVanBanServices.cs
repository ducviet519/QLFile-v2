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
        public Task<List<BangTinVanBan>> Get_BangTinVanBan(string user, string loaivb = null);
        public Task<List<PreviewVanBan>> Preview_VanBan(string user, string idvanban = null);
        public Task<List<VanBanChiTiet>> Table_VanBanChiTiet(Search_VanBanChiTiet search = null);
        public Task<List<VanBanChiTiet>> Table_VanBanChiTiet_Google(Search_VanBanChiTiet search = null, List<GoogleDriveFile> table = null);
        public Task<string> ViewLog(string idvb, string user);
        public Task<string> SendLike(string idvb, string user);
        public Task<List<VanBan_PhienBan>> Get_DanhSachPhienBan(string idvb);
        public Task<string> Upsert_PhienBan(VanBan_PhienBan phienban, string user);
        public Task<string> StatUpdate_PhienBan(string idvb, string idphienban, int cmdtype, string user);
        public Task<string> Upsert_VanBan_ThuMuc(List<VanBan_ID> listID, int IDCabinet, string user);
        public Task<List<VanBan_ListName>> Get_ListTenVanBan(string text = null);
        public Task<List<VanBan_ByID>> Get_VanBanByID(string docID = null);
        public Task<string> Add_VanBan(VanBan_CRUD vanban, List<VanBan_ID> listID, string user);
        public Task<string> Update_VanBan(VanBan_CRUD vanban, List<VanBan_ID> listID, string user);
        public Task<string> Delete_VanBan(string idvb);
    }
}
