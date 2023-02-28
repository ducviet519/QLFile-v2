using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models.Entities
{
    public class FileImport
    {
        public string status { get; set; }
        public string filepath { get; set; }
        public DataTable dataExcels { get; set; }
    }

    public class FileData
    {
        public int stt { get; set; }
        public string TenLoaiVanBan { get; set; }
        public string LoaiVanBan { get; set; }
        public string TenVanBan { get; set; }
        public string MaVanBan { get; set; }
        public string NgayBanHanhText { get; set; }
        public string NgayHieuLucText { get; set; }
        public string NgayBanHanh { get; set; }
        public string NgayHieuLuc { get; set; }
        public string RenamedFileName { get; set; }
        public string PhienBanText { get; set; }
        public string PhienBan { get; set; }
        public string TheLoaiBMText { get; set; }
        public string TheLoaiBM { get; set; }
        public string Cabinet { get; set; }
        public string CabinetText { get; set; }
        public string Watemark { get; set; }
        public string IDVanBan { get; set; }
        public string IDPhienBan { get; set; }
        public string IDFileLink { get; set; }

    }

    public class FileSend
    {
        public string LoaiVanBan { get; set; }
        public string TenVanBan { get; set; }
        public string MaVanBan { get; set; }
        public string NgayBanHanh { get; set; }
        public string NgayHieuLuc { get; set; }
        public string TenFile { get; set; }
        public string PhienBan { get; set; }
        public string TheLoaiBM { get; set; }
        public string Cabinet { get; set; }
        public string Watermark { get; set; }
        public string IDVanBan { get; set; }
        public string IDPhienBan { get; set; }
        public string IDFileLink { get; set; }
    }

}
