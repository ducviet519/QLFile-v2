﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using System.Drawing;
using System.Diagnostics;

namespace WebTools.Controllers
{
    public class DocEditerController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public DocEditerController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            string filePath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot\\files\\Form email PHVB.docx");
            ViewBag.Document = filePath;
            return View();
        }

        public IActionResult Export(string base64, string fileName, DevExpress.AspNetCore.RichEdit.DocumentFormat format)
        {
            string documentFolderPath = "D:\\VanBan\\";
            byte[] fileContents = System.Convert.FromBase64String(base64);
            System.IO.File.WriteAllBytes($"{documentFolderPath}{fileName}.{GetExtension(format)}", fileContents);
            return new EmptyResult();
        }

        private static string GetExtension(DevExpress.AspNetCore.RichEdit.DocumentFormat format)
        {
            switch (format.ToString())
            {
                case "OpenXml": return "docx";
                case "Rtf": return "rtf";
                case "PlainText": return "txt";
            }
            return "docx";
        }

        public IActionResult DocumentProtection()
        {
            return View();
        }

        public IActionResult Document()
        {
            string fileName = "Test.docx";
            using (var wordProcessor = new RichEditDocumentServer())
            {
                Document doc = wordProcessor.Document;
                doc.AppendText("This document is generated by Word Processing Document API");
                CharacterProperties cp =
                     doc.BeginUpdateCharacters(doc.Paragraphs[0].Range);
                cp.ForeColor = Color.FromArgb(0x83, 0x92, 0x96);
                cp.Italic = true;
                doc.EndUpdateCharacters(cp);

                ParagraphProperties pp =
                     doc.BeginUpdateParagraphs(doc.Paragraphs[0].Range);
                pp.Alignment = ParagraphAlignment.Right;
                doc.EndUpdateParagraphs(pp);
                wordProcessor.SaveDocument(fileName, DevExpress.XtraRichEdit.DocumentFormat.OpenXml);
            }
            Process.Start(new ProcessStartInfo(fileName) { UseShellExecute = true });
            return View();
        }
    }
}
