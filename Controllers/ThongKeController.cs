using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebTools.Models.Entities;
using WebTools.Models.ViewModel;
using WebTools.Services;

namespace WebTools.Controllers
{
    [Authorize]
    public class ThongKeController : Controller
    {
        #region Services
        private readonly IUnitOfWork _services;
        public ThongKeController(IUnitOfWork services)
        {
            _services = services;
        } 
        #endregion

        #region Thống kê chi tiết theo phạm vi văn bản
        public IActionResult ThongKePhamViVanBan()
        {
            ThongKeVanBanVM model = new ThongKeVanBanVM();
            return View(model);
        }

        [HttpGet]
        public async Task<JsonResult> GetTable_ThongKePhamViVanBan(string loai, string tungay = null, string denngay = null, string doituongapdung = null, string donviapdung = null, string phamvi = null, string loaivanban = null)
        {
            var data = (await _services.Report_List.GetReportListAsync()).Take(10).ToList();
            return Json(new { data });
        }
        #endregion

        #region Thống kê số lượng user đã đọc và hiểu nội dung văn bản
        public IActionResult ThongKeSoLuongXacNhan()
        {
            ThongKeVanBanVM model = new ThongKeVanBanVM();
            return View(model);
        }

        [HttpGet]
        public async Task<JsonResult> GetTable_ThongKeSoLuongXacNhan(string loai, string tungay = null, string denngay = null, string doituongapdung = null, string donviapdung = null, string loaivanban = null)
        {
            var data = (await _services.Report_List.GetReportListAsync()).Take(10).ToList();
            return Json(new { data });
        }
        #endregion

        #region Báo cáo thống kê tổng hợp
        public IActionResult ThongKeTongHop()
        {
            return View();
        }

        public async Task<JsonResult> GetTable_ThongKeTongHop(string loai, string tungay = null, string denngay = null, string doituongapdung = null, string donviapdung = null)
        {
            var data = (await _services.Report_List.GetReportListAsync()).Take(10).ToList();
            return Json(new { data });
        }
        #endregion

        #region Biểu đồ
        public async Task<IActionResult> BieuDoThongKe(string loaibc = null)
        {
            ThongKeVM model = new ThongKeVM();
            var data = (await _services.ThongKe.Get_DataBieuDo()).GroupBy(i => new { i.LoaiBC, i.TenBC }).ToList();
            List<ThongKe_BieuDo> list = new List<ThongKe_BieuDo>();
            foreach (var key in data)
            {
                ThongKe_BieuDo item = new ThongKe_BieuDo() { LoaiBC = key.Key.LoaiBC, TenBC = key.Key.TenBC};
                list.Add(item);
            }
            model.ListBieuDo = list;
            model.countBC = data.Count;
            return View(model);
        }

        [HttpGet]
        public async Task<JsonResult> Get_DashboardData(string loaibc = null)
        {
            var data = (await _services.ThongKe.Get_DataBieuDo()).Where(i => i.LoaiBC.Contains(loaibc));
            return Json(new { data });
        }
        #endregion
    }
}
