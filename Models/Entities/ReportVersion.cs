﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models
{
    public class ReportVersion
    {
        public string IDPhienBan { get; set; }

        [Display(Name = "Ngày ban hành")]
        public string NgayBanHanh { get; set; }
        [Display(Name = "Ghi chú")]
        public string GhiChu { get; set; }

        [Display(Name = "File biểu mẫu")]
        public string FileLink { get; set; }

        [Display(Name = "Phiên bản")]
        public string PhienBan { get; set; }

        [Display(Name = "Trạng thái phần mềm")]
        public string TrangThaiPM { get; set; }

        [Display(Name = "Trạng thái sử dụng")]
        public string TrangThaiSD { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedUser { get; set; }

        public string IDBieuMau { get; set; }

        [Display(Name = "Tên biểu mẫu")]        
        public string TenBM { get; set; }

        [Display(Name = "Mã biểu mẫu")]
        public string MaBM { get; set; }
        public string NgayBanHanhNew { get; set; }

        [Display(Name = "STT")]
        public int STT { get; set; }

        [Display(Name = "Trạng thái")]
        public string TrangThai { get; set; }
        public int SoPB { get; set; }
        public string User { get; set; }

        [Required(ErrorMessage = "Chọn một file")]
        [DataType(DataType.Upload)]
        //[FileExtensions(Extensions = "png,jpg,jpeg,gif,doc,docx,pdf,txt")]
        [Display(Name = "File:")]
        [BindProperty]
        public IFormFile fileUpload { get; set; }

    }
}
