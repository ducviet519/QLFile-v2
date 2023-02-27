using ClosedXML.Excel;
using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;
using WebTools.Services.Interface;

namespace WebTools.Services
{
    public class UploadFileServices : IUploadFileServices
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        public UploadFileServices(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("ToolsDB");
            provideName = "System.Data.SqlClient";
        }
        public string ConnectionString { get; }
        public string provideName { get; }
        public IDbConnection Connection
        {
            get { return new SqlConnection(ConnectionString); }
        }
        public string DeleteFile(string filePath)
        {
            string result = "";
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
            {
                file.Delete();
                result = "Đã xóa file thành công";
            }
            else
            {
                result = "Lỗi! Xóa file không thành công";
            }
            return result;
        }

        public async Task<string> FileNameCache(string NewFileName, string OldFileName)
        {
            string result = String.Empty;
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = await dbConnection.ExecuteAsync("sp_Report_FilenameCache_Upsert",
                        new
                        {
                            OldFileName = OldFileName,
                            RenamedFileName = NewFileName
                        },
                        commandType: CommandType.StoredProcedure);
                    if (data > 0)
                    {
                        result = "OK";
                    }
                    dbConnection.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }
        }

        public async Task<FileImport> ReadExcelFile(IFormFile fileUpload)
        {
            FileImport data = new FileImport();
            try
            {
                string randomID = System.Guid.NewGuid().ToString("N");
                string[] allowedExtsnions = new string[] { ".xls", ".xlsx", ".csv" };
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "files");
                if (!Directory.Exists(uploadsFolder)) { Directory.CreateDirectory(uploadsFolder); }
                if (fileUpload != null && fileUpload.Length > 0)
                {
                    string dataFileName = Path.GetFileName(fileUpload.FileName);
                    string extension = Path.GetExtension(dataFileName);
                    if (!allowedExtsnions.Contains(extension)) { data.status = "File tải lên không phải file Excel"; data.dataExcels = null; }
                    else
                    {
                        string fileName = $"{randomID}_{fileUpload.FileName}";
                        string filePath = Path.Combine(uploadsFolder, fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                        {
                            await fileUpload.CopyToAsync(fileStream);
                        }
                        if (File.Exists(filePath))
                        {
                            XLWorkbook workbook = XLWorkbook.OpenFromTemplate(filePath);
                            var sheets = workbook.Worksheets.First();
                            
                            //var sheets = workbook.Worksheet(1).RangeUsed().RowsUsed().Skip(1);
                            var rows = sheets.RangeUsed().RowsUsed().Skip(1).ToList();
                            DataTable table = new DataTable();
                            for(int i = 1; i <= 50; i++)
                            {
                                table.Columns.Add(new DataColumn($"Col{i}", typeof(string)));
                            }                                                      
                            foreach (var row in rows)
                            {
                                DataRow newRow = table.NewRow();
                                    for(int i = 1; i <= 50; i++)
                                    {
                                        newRow[$"Col{i}"] = row.Cell(i).Value.ToString();
                                    }
                                table.Rows.Add(newRow);
                            }

                            data.status = "OK";
                            data.filepath = filePath;
                            data.dataExcels = table;
                            foreach (DataRow row in table.Rows)
                            {
                                var expando = new ExpandoObject() as IDictionary<string, Object>;
                                foreach (DataColumn col in table.Columns)
                                {
                                    expando.Add(col.ColumnName, row[col.ColumnName]);
                                }
                            }
                            DataRow test = table.Rows[0];
                            File.Delete(filePath);
                        }
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                string Msg = ex.Message;
                return data;
            }
        }

        public async Task<string> UploadFileAsync(IFormFile fileUpload)
        {
            string FileLink = "";
            string getDateS = DateTime.Now.ToString("ddMMyyyyHHmmss");
            string uploadsFolder = "D:\\VanBan";
            if (!Directory.Exists(uploadsFolder)) { Directory.CreateDirectory(uploadsFolder); }

            if (fileUpload != null && fileUpload.Length > 0)
            {
                string fileName = $"{getDateS}_{fileUpload.FileName}";
                string filePath = Path.Combine(uploadsFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    await fileUpload.CopyToAsync(fileStream);
                }
                if (File.Exists(filePath))
                {
                    return FileLink = filePath;
                }
                else { return FileLink = ""; }
            }
            else
                return FileLink = "";
        }

    }
}
