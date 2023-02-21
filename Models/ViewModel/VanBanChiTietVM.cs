using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;

namespace WebTools.Models.ViewModel
{
    public class VanBanChiTietVM
    {
        public Search_VanBanChiTiet SearchList { get; set; }
        public List<VanBan_PhienBan> ListPhienBan { get; set; }
        public VanBan_PhienBan VanBanInfo { get; set; }
        public string idvb { get; set; }
        public string idphienban { get; set; }
        public string trangthai { get; set; }

        public bool themphienban { get; set; }
        public bool suavanban { get; set; }
        public bool phanquyenvanban { get; set; }
        public bool xoavanban { get; set; }
    }
}
