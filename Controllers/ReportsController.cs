using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Services;

namespace WebTools.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IUnitOfWork _services;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ReportsController(IWebHostEnvironment webHostEnvironment, IUnitOfWork services)
        {
            _services = services;
            _webHostEnvironment = webHostEnvironment;
            Stimulsoft.Base.StiLicense.Key = "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHmKmIYtQbGVffUafn4grxXukjNz+mLI/d2lO8A9DuHxAxu4MmrN4lqhtklB0Pcnfq+AIk+1s6kU0D6JIIxpfQs1FV6pzwrzAWDLexRPvVgpswR/C5n69gFjFmWmtV0bGCTAj6H/MHgwElWYT9nYccmuFH0sfAj7VELa2HehqREJ7di+05AWeWIScfcWsf0XZ3JK+2X3reYvhUEAxrCwX3FDw2p1hPQIfiUVkUd4NRvfwWBLOjP2ZTfp9dzNe1D6T7miK1VBd731o/ex6q1bkdNH+G1KV6OLxdQwb9zWr4AR0QtMKwrphskgAYb5PMCectoVCeRFKtWSm8qnMP+GN8v0GHbJGQ/ZYWMAsE94nCEhnJz9T/xfy1b6WEWqpUUjVsfPzm/naRz0bxVN+AtFW7thTBMwGXoKTJXnKhfrI5u/fRffUu78ejnazBjWCHANN/xF7XOLhnFWw5JZHPXkaoCbiG8kBL3bgL4DtSyZ7jecAPmJUtainwLNMdo75J6hvu/pS+jDUs2tqRjEv6PTehXg";
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetReport()
        {
            var report = StiReport.CreateNewReport();
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "Reports\\DemoReport.mrt");
            using (var stream = System.IO.File.OpenRead(path))
            {
                report.Load(stream);
            }

            var objQuyenLoaiVB = await _services.DanhMuc.Get_DM_QuyenLoaiVB();
            report.RegData("QuyenLoaiVB", objQuyenLoaiVB);
            return StiNetCoreViewer.GetReportResult(this, report);
        }

        public IActionResult ViewerEvent()
        {
            return StiNetCoreViewer.ViewerEventResult(this);
        }

        //@Html.ActionLink("Export to Pdf", "ExportReport")
        public async Task<IActionResult> ExportReport()
        {
            var report = StiReport.CreateNewReport();
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "Reports\\DemoReport.mrt");
            using (var stream = System.IO.File.OpenRead(path))
            {
                report.Load(stream);
            }

            var objQuyenLoaiVB = await _services.DanhMuc.Get_DM_QuyenLoaiVB();
            report.RegBusinessObject("QuyenLoaiVB", objQuyenLoaiVB);
            //report.RegData("QuyenLoaiVB", objQuyenLoaiVB); //Json

            return StiNetCoreReportResponse.ResponseAsPdf(report);
        }
    }
}
