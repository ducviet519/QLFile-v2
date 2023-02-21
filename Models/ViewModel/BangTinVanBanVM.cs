using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;

namespace WebTools.Models.ViewModel
{
    public class BangTinVanBanVM
    {
        public List<DanhMuc> LoaiVanBanHienThi { get; set; }
        public List<DanhMuc> LoaiCabinet { get; set; }
        public IEnumerable<SelectListItem> LoaiVanBan { get; set; }
    }
}
