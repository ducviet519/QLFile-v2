using GleamTech.DocumentUltimate.AspNet.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;

namespace WebTools.Models
{
    public class DocumentViewModel
    {
        public DocumentViewer DocumentViewer { get; set; }
        public string FileLink { get; set; }
        public string FileName { get; set; }
        public string FullName { get; set; }
        public string Extension { get; set; }
        public string IDPhienBan { get; set; }
    }

    public class DocumentVM
    {
        public PreviewVanBan fileInfo { get; set; }
        public string Iframe { get; set; }
        public string user { get; set; }
        public List<PreviewVanBan> listVanBanLienQuan { get; set; }

    }

    public class VanBanLienQuan
    {
        public string vanbanID { get; set; }
        public string tenvanban { get; set; }
        public string linkvanban { get; set; }
    }
}
