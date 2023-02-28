using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebTools.Models.Entities;
using WebTools.Services;

namespace WebTools.Controllers
{
    public class UploadController : Controller
    {
        private readonly IUnitOfWork _services;
        public UploadController(IUnitOfWork services)
        {
            _services = services;
        }
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
                    string getDateS = Guid.NewGuid().ToString("N");
                    //string getDateS = DateTime.Now.ToString("ddMMyyyyHHmmss");
                    try
                    {
                        string msg = String.Empty;
                        string uploadsFolder = "D:\\VanBan";
                        if (!Directory.Exists(uploadsFolder)) { Directory.CreateDirectory(uploadsFolder); }
                        string fileOldName = file.FileName;
                        string fileName = $"{getDateS}_{file.FileName}";
                        string filePath = Path.Combine(uploadsFolder, fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        if (System.IO.File.Exists(filePath))
                        {
                            msg = "success";
                            await _services.UploadFile.FileNameCache(fileName, fileOldName);
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

        [HttpPost]
        public async Task<JsonResult> UploadExcelFile(IFormFile fileUpload)
        {
            string message = String.Empty;
            string title = String.Empty;
            string result = String.Empty;
            FileImport data = new FileImport();
            List<FileData> filesData = new List<FileData>();
            string user = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.GivenName).Value ?? HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            try
            {
                if (fileUpload != null)
                {
                    data = await _services.UploadFile.ReadExcelFile(fileUpload);                   
                    if (data.status == "OK")
                    {
                        var filesDataRes = (await _services.UploadFile.FileImport("1", data.dataExcels, user)).ToList();
                        int count = 0;
                        foreach(var fileData in filesDataRes)
                        {
                            FileData dataex = fileData;
                            dataex.stt = count++;
                            filesData.Add(dataex);
                        }
                        message = $"Lấy thành công {data.dataExcels.Rows.Count} văn bản";
                        title = "Thành công!";
                        result = "success";
                    }
                    else
                    {
                        message = data.status;
                        title = "Lỗi!";
                        result = "error";
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                title = "Lỗi!";
                result = "error";
            }
            return Json(new { Result = result, Title = title, Message = message, file = filesData });
        }

        [HttpPost]
        public async Task<JsonResult> DataExcel([FromBody]List<FileData> listFiles)
        {
            string message = String.Empty;
            string title = String.Empty;
            string result = String.Empty;
            string user = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.GivenName).Value ?? HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            try
            {
                result = await _services.UploadFile.UpsertDataExcel(user, listFiles);
                if (result == "OK")
                {
                    message = $"Lưu thành công";
                    title = "Thành công!";
                    result = "success";

                }
                else
                {
                    message = result;
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
