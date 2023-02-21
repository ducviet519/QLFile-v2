using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;

namespace WebTools.Models.ViewModel
{
    public class PhanQuyenVM
    {
        public DanhMuc_HRMUsers Users { get; set; }
        public List<Roles> ListRoles { get; set; }
        public Roles Role { get; set; }
        public IEnumerable<SelectListItem> Area { get; set; }
    }
}
