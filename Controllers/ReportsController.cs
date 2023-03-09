using GleamTech.DocumentUltimate.AspNet.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;
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
        }

        public class FileReport
        {
            public string filePath { get; set; }
        }
        public class TestModel
        {
            public string ID { get; set; }
            public string MaBM { get; set; }
            public string TenBM { get; set; }
            public string URD { get; set; }
            public string KhoaPhong { get; set; }
        }       

        public async Task<IActionResult> GetReport(string reportName, string jsonContent)
        {
            ViewBag.FilePath = JsonConvert.DeserializeObject<FileReport>(await _services.ReportsAPI.GetReport(reportName, jsonContent)).filePath;
            return View();
        }

        public IActionResult Overview(string filePath)
        {
            var documentViewer = new DocumentViewer
            {
                Width = 1100,
                Height = 600,
                Resizable = true,
                Document = filePath
            };

            return View(documentViewer);
        }       

        
    }
}
