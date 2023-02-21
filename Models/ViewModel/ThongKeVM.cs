using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;

namespace WebTools.Models.ViewModel
{
    public class ThongKeVM
    {
        public int countBC { get; set; }
        public ThongKe_BieuDo BieuDoData { get; set; }
        public List<ThongKe_BieuDo> ListBieuDo { get; set; }
    }
}
