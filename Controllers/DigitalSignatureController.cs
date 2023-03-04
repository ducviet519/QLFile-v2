using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using GleamTech.DocumentUltimate;
using Jose;
using Microsoft.AspNetCore.Mvc;
using Spire.Pdf;
using Spire.Pdf.General.Find;
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

        #region Convert file to PDF and Sign
        private async Task<FileSignPDF> SignPDFWithImage(string filePath = null, string imagePath = null)
        {
            List<SigningRequestContents> ListFilesData = new List<SigningRequestContents>();
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            var textCoordinates = GetTextCoordinates("(Ký, ghi rõ họ tên)", filePath);
            if (textCoordinates == null) { return new FileSignPDF(); }
            SigningRequestContents fileData = new SigningRequestContents()
            {
                documentName = fileName,
                data = StaticHelper.EncodeFileToBase64(filePath),
                location = new RequestContentsLocation()
                {
                    visibleX = (textCoordinates.X - 50) < 0 ? 0 : (textCoordinates.X - 50),
                    visibleY = (textCoordinates.Y - 50) < 0 ? 0 : (textCoordinates.Y - 50),
                    visibleWidth = 160,
                    visibleHeight = 100
                },
                extraInfo = new RequestContentsExtraInfo() { pageNum = textCoordinates.pageIndex },
                imageSignature = StaticHelper.EncodeFileToBase64(imagePath)
            };
            ListFilesData.Add(fileData);
            var responseData = await _services.DigitalSign.DigitalSignaturePDF(ListFilesData);
            FileSignPDF fileSign = new FileSignPDF()
            {
                fileName = fileName,
                filePath = StaticHelper.DecodeBase64ToFilePDF(responseData.data, fileName, "D:\\SignFileOutput")
            };
            return fileSign;
        }
        private async Task<List<FileSignPDF>> SignPDFInviciable(List<FileSignPDF> listFilePDF)
        {
            List<FilePDFInviciablContents> ListFilesData = new List<FilePDFInviciablContents>();
            List<FileSignPDF> listFileSigned = new List<FileSignPDF>();
            foreach (var filePDF in listFilePDF)
            {
                FilePDFInviciablContents fileData = new FilePDFInviciablContents()
                {
                    documentName = Path.GetFileNameWithoutExtension(filePDF.filePath),
                    data = StaticHelper.EncodeFileToBase64(filePDF.filePath),
                };
                ListFilesData.Add(fileData);
            }
            var responseData = await _services.DigitalSign.InvisiableSignaturePDF(ListFilesData);
            if (responseData.status == 0)
            {
                foreach (var file in responseData.data.responseContentList)
                {
                    FileSignPDF fileSigned = new FileSignPDF()
                    {
                        fileName = file.documentName,
                        filePath = StaticHelper.DecodeBase64ToFilePDF(file.signedDocument, file.documentName, "D:\\SignFileOutput"),
                    };
                    listFileSigned.Add(fileSigned);
                }
            }
            return listFileSigned;
        }
        private TextCoordinates GetTextCoordinates(string textFind, string filePath)
        {
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(filePath);

            PdfTextFind[] results = null;
            List<TextCoordinates> listPosition = new List<TextCoordinates>();
            foreach (PdfPageBase page in doc.Pages)
            {
                results = page.FindText(textFind).Finds;
                foreach (PdfTextFind text in results)
                {
                    TextCoordinates p = new TextCoordinates()
                    {
                        X = (int)Math.Floor(text.Position.X),
                        Y = (int)Math.Floor(page.ActualSize.Height - text.Position.Y),
                        pageIndex = text.SearchPageIndex + 1,
                    };
                    listPosition.Add(p);
                }
            }
            var data = listPosition;
            return listPosition.LastOrDefault();
        }
        private string DocConvert(string filePathInput, string uploadsFolder)
        {
            string randomID = Guid.NewGuid().ToString("N");
            if (!Directory.Exists(uploadsFolder)) { Directory.CreateDirectory(uploadsFolder); }
            string fileName = System.IO.Path.GetFileNameWithoutExtension(filePathInput);
            string filePathOutput = Path.Combine(uploadsFolder, $"{randomID}_{fileName}.pdf");
            DocumentConverter.Convert(filePathInput, filePathOutput);
            if (System.IO.File.Exists(filePathOutput))
            {
                return filePathOutput;
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion

        public JsonResult Index()
        {
            //string filePathInput = "D:\\VanBan\\BM.02.QLCL.57.V1. Bien ban ban giao mau phoi tru dong.docx";
            //string fileOutput = DocConvert(filePathInput, "D:\\SignFileOutput");

            //string imagePath = "C:\\Users\\vietld\\Downloads\\logo_300x300.png";
            //var signImageOutput = SignPDFWithImage(fileOutput, imagePath);

            //List<FileSignPDF> listFilePDF = new List<FileSignPDF>() 
            //{ 
            //    new FileSignPDF() { filePath = fileOutput } 
            //};
            //var signInviableOuput = SignPDFInviciable(listFilePDF);

            //return Json(new { filePath = fileOutput, signImageOutput = signImageOutput, signInviableOuput = signInviableOuput });
            return Json(new { });
        }
    }
}
