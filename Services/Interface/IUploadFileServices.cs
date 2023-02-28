using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;

namespace WebTools.Services.Interface
{
    public interface IUploadFileServices
    {
        Task<string> UploadFileAsync(IFormFile fileUpload);
        string DeleteFile(string filePath);
        Task<FileImport> ReadExcelFile(IFormFile fileUpload);
        Task<string> FileNameCache(string NewFileName, string OldFileName);
        Task<List<FileData>> FileImport(string loaiFile, DataTable table, string user);
        Task<string> UpdateFileLink(string IDFileLink, string FileLink);
        Task<string> UpsertDataExcel(string user, List<FileData> listFiles);
    }
}
