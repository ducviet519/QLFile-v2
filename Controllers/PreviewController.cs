using GleamTech.DocumentUltimate;
using GleamTech.DocumentUltimate.AspNet;
using GleamTech.DocumentUltimate.AspNet.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebTools.Models;
using WebTools.Models.Entities;
using WebTools.Services;

namespace WebTools.Controllers
{

    [Authorize]
    public class PreviewController : Controller
    {
        #region Services
        private readonly IUnitOfWork _services;
        private readonly IWebHostEnvironment _hosting;
        public PreviewController(IUnitOfWork services, IWebHostEnvironment hosting)
        {
            _services = services;
            _hosting = hosting;
        }
        #endregion

        #region Kiểm tra quyền người dùng
        private bool PermissionInUser(string permission)
        {
            var data = HttpContext.User.Claims.Where(c => c.Type == "Permission" && c.Value.ToUpper().Contains(permission.ToUpper())).ToList();
            if (data != null && data.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Đọc file và trả preview băn bản về View
        private DocumentViewModel GetDocumentViewer(int? downloaded, int? printed, int? watermark, string documentLink = null, string textmarks = null, string imagemarksPath = null)
        {
            DocumentViewModel model = new DocumentViewModel();
            model.FileName = System.IO.Path.GetFileName(documentLink);

            if (String.IsNullOrEmpty(documentLink)) { return model = null; }

            var documentOptions = new DocumentViewer
            {
                Width = 1100,
                Height = 600,
                Resizable = false,
                Document = documentLink,
                Watermarks = { },
                ClientEvents = new DocumentViewerClientEvents
                {
                    Loaded = "documentViewerLoaded",
                    PageChanged = "documentViewerPageChanged",
                    PageRendered = "documentViewerPageRendered",
                }
            };
            if (watermark > 0)
            {
                //Thêm Watermarks                      
                if (!String.IsNullOrEmpty(imagemarksPath) && String.IsNullOrEmpty(textmarks))
                {
                    ImageWatermark imageWatermark = new ImageWatermark()
                    {
                        ImageFile = imagemarksPath,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Opacity = 20,
                        Height = 100,
                        PageRange = "All",
                    };
                    documentOptions.Watermarks.Add(imageWatermark);
                }
                if (!String.IsNullOrEmpty(textmarks))
                {
                    TextWatermark textWatermark = new TextWatermark()
                    {
                        Text = textmarks,
                        Rotation = -45,
                        Opacity = 10,
                        FontColor = Color.Red,
                        Width = 65,
                        Height = 65,
                        SizeIsPercentage = true,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                    };
                    documentOptions.Watermarks.Add(textWatermark);
                }
            }
            //Phân quyền           
            if (downloaded < 1 && printed < 1) 
            {
                documentOptions.AllowedPermissions = DocumentViewerPermissions.All;
                documentOptions.DeniedPermissions = DocumentViewerPermissions.Download | DocumentViewerPermissions.DownloadAsPdf | DocumentViewerPermissions.Print;
            }
            else if (downloaded < 1 && printed >= 1) 
            {
                documentOptions.AllowedPermissions = DocumentViewerPermissions.All;
                documentOptions.DeniedPermissions = DocumentViewerPermissions.Download | DocumentViewerPermissions.DownloadAsPdf;
            }
            else if (downloaded >= 1 && printed < 1) 
            {
                documentOptions.AllowedPermissions = DocumentViewerPermissions.All;
                documentOptions.DeniedPermissions = DocumentViewerPermissions.Print;
            }
            else
            {
                documentOptions.AllowedPermissions = DocumentViewerPermissions.All;
            }
            model.DocumentViewer = documentOptions;
            return model;
        }

        public IActionResult GetFile(int readed, int downloaded, int printed, int? watermark, string filePath = null, string imagemarksPath = null, string textmarks = null, string type = null)
        {
            if (String.IsNullOrEmpty(imagemarksPath)) { imagemarksPath = Path.Combine(_hosting.WebRootPath, "images\\logo_300x300.png"); }
            //if (String.IsNullOrEmpty(textmarks)) { textmarks = "Tâm Anh Hospital"; }
            if (readed < 1)
            {
                return PartialView("_Denied");
            }
            if (String.IsNullOrEmpty(filePath) || filePath == "null")
            {
                return PartialView("_NoFilePreview");
            }
            DocumentViewModel model = GetDocumentViewer(downloaded, printed, watermark, filePath, textmarks, imagemarksPath);
            return View(model);
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> VanBan(string file = null)
        {
            DocumentVM model = new DocumentVM();
            string user = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.GivenName).Value ?? HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            model.user = user;
            var listfile = (await _services.VanBan.Preview_VanBan(user, file)).ToList();
            if (listfile != null && listfile.Count > 0)
            {
                PreviewVanBan fileInfo = listfile.FirstOrDefault();
                model.fileInfo = fileInfo;
                listfile.RemoveAt(0);
                model.listVanBanLienQuan = listfile;
            }
            else
            {
                PreviewVanBan fileInfo = new PreviewVanBan()
                {
                    TenVB = "",
                    Downloaded = 0,
                    FileLink = "null",
                    IDVB = file,
                    Printed = 0,
                    Readed = 1,
                    YeuThich = 0,
                    Watermark = 1
                };
                model.fileInfo = fileInfo;
                model.listVanBanLienQuan = listfile;
            }
            return View(model);
        }

        public IActionResult VanBanKy(string file = null)
        {
            return View();
        }

        public IActionResult VanBan_XacNhan(string idvb = null)
        {
            PreviewVanBan model = new PreviewVanBan();
            model.IDVB = idvb;
            return PartialView("_VanBan_XacNhan", model);
        }

        [HttpPost]
        public async Task<JsonResult> Send_ViewLog(string idvb)
        {
            string message = "";
            string title = "";
            string result = "";
            string user = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.GivenName).Value ?? HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            try
            {
                result = await _services.VanBan.ViewLog(idvb, user);
                if (result == "OK")
                {
                    message = $"Thành công";
                    title = "Thành công!";
                    result = "success";
                }
                else
                {
                    message = $"Lỗi! {result}";
                    title = "Lỗi!";
                    result = "error";
                }

            }
            catch (Exception ex)
            {
                message = ex.Message;
                title = "Lỗi!";
                result = "error";
            }
            return Json(new { Result = result, Title = title, Message = message });
        }

        public IActionResult Preview(int readed, string filePath, string filename = null)
        {
            DocumentVM model = new DocumentVM();
            model.fileInfo = new PreviewVanBan()
            {
                Readed = readed,
                FileLink = filePath,
                TenVB = filename
            };
            return PartialView("_PreviewPartial", model);
        }

        public async Task<JsonResult> Send_Like(string idvb)
        {
            string message = "";
            string title = "";
            string result = "";
            string user = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.GivenName).Value ?? HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            try
            {
                result = await _services.VanBan.SendLike(idvb, user);
                if (result == "OK")
                {
                    message = $"Thành công";
                    title = "Thành công!";
                    result = "success";
                }
                else
                {
                    message = $"Lỗi! {result}";
                    title = "Lỗi!";
                    result = "error";
                }

            }
            catch (Exception ex)
            {
                message = ex.Message;
                title = "Lỗi!";
                result = "error";
            }
            return Json(new { Result = result, Title = title, Message = message });
        }
    }
}
