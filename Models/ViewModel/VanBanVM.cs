using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;

namespace WebTools.Models.ViewModel
{
    public class VanBanVM
    {
        public VanBan_ByID vanbanInfo { get; set; }
        public List<VanBan_ByID> listVanBan { get; set; }
        public IEnumerable<SelectListItem> selectTheLoai { get; set; }
        public IEnumerable<SelectListItem> selectDonVi { get; set; }
        public IEnumerable<SelectListItem> selectDoiTuong { get; set; }
    }
}
