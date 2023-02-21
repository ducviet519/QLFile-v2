using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;

namespace WebTools.Models.ViewModel
{
    public class ThongKeVanBanVM
    {
        public IEnumerable<SelectListItem> loaivanban { get; set; }
        public IEnumerable<SelectListItem> phamvi { get; set; }
    }
}
