using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebTools.Controllers
{
    public class UploadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<JsonResult> UploadMultipleFile()
        {
            var files = HttpContext.Request.Form.Files;
            string[] messages = { };          
            if (files.Any())
            {
                foreach (var file in files)
                {
                    try
                    {
                        string msg = String.Empty;
                        string getDateS = DateTime.Now.ToString("ddMMyyyyHHmmss");
                        string uploadsFolder = "D:\\VanBan";
                        if (!Directory.Exists(uploadsFolder)) { Directory.CreateDirectory(uploadsFolder); }
                        string fileName = $"{getDateS}_{file.FileName}";
                        string filePath = Path.Combine(uploadsFolder, fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        if (System.IO.File.Exists(filePath))
                        {
                            msg = "success";
                        }
                        else
                        {
                            msg = "error";
                        }
                        messages.Append(msg);
                    }
                    catch (Exception ex)
                    {
                        messages.Append(ex.Message);
                        throw;
                    }
                }
            }
            return Json( new { status = true, messages = messages });
        }
    }
}
