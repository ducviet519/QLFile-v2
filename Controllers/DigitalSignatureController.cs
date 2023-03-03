using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Jose;
using Microsoft.AspNetCore.Mvc;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using Spire.Pdf.Security;
using WebTools.Extensions;
using WebTools.Models.Entities;
using WebTools.Services;
using static WebTools.Extensions.StaticHelper;

namespace WebTools.Controllers
{
    public class DigitalSignatureController : Controller
    {
        #region Services
        private readonly IUnitOfWork _services;
        public DigitalSignatureController(IUnitOfWork services)
        {
            _services = services;
        }
        #endregion

        public async Task<JsonResult> SignPDFWithImage(string filePath = null, string imagePath = null)
        {
            filePath = "D:\\VanBan\\license_en.pdf";
            imagePath = "C:\\Users\\vietld\\Downloads\\logo_300x300.png";

            List<SigningRequestContents> ListFilesData = new List<SigningRequestContents>();
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            SigningRequestContents fileData = new SigningRequestContents()
            {
                documentName = fileName,
                data = StaticHelper.EncodeFileToBase64(filePath),
                location = new RequestContentsLocation()
                {
                    visibleX = 100,
                    visibleY = 100,
                    visibleWidth = 100,
                    visibleHeight = 60
                },
                extraInfo = new RequestContentsExtraInfo() { pageNum = 1 },
                imageSignature = StaticHelper.EncodeFileToBase64(imagePath)
            };
            ListFilesData.Add(fileData);
            var responseData = await _services.DigitalSign.DigitalSignaturePDF(ListFilesData);
            FileSignPDF fileSign = new FileSignPDF()
            {
                fileName = fileName,
                filePath = StaticHelper.DecodeBase64ToFilePDF(responseData.data, fileName)
            };
            return Json(new { data = fileSign });
        }

        public async Task<JsonResult> SignPDFInviciable(string filePath = null)
        {
            filePath = "D:\\VanBan\\license_en.pdf";
            List<FilePDFInviciablContents> ListFilesData = new List<FilePDFInviciablContents>();
            FilePDFInviciablContents fileData = new FilePDFInviciablContents()
            {
                documentName = Path.GetFileNameWithoutExtension(filePath),
                data = StaticHelper.EncodeFileToBase64(filePath),
            };
            ListFilesData.Add(fileData);
            var responseData = await _services.DigitalSign.InvisiableSignaturePDF(ListFilesData);
            List<FileSignPDF> listFileSigned = new List<FileSignPDF>();
            if (responseData.status == 0)
            {
                foreach (var file in responseData.data.responseContentList)
                {
                    FileSignPDF fileSigned = new FileSignPDF()
                    {
                        fileName = file.documentName,
                        filePath = StaticHelper.DecodeBase64ToFilePDF(file.signedDocument, file.documentName),
                    };
                    listFileSigned.Add(fileSigned);
                }
            }
            return Json(new { data = listFileSigned });
        }
    }
}
