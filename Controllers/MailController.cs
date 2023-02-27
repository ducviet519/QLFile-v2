using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;
using WebTools.Services;

namespace WebTools.Controllers
{
    public class MailController : Controller
    {
        #region Services
        private readonly IUnitOfWork _services;
        public MailController(IUnitOfWork services)
        {
            _services = services;
        }
        #endregion

        public IActionResult MailTemplate(int type)
        {
            ViewBag.TenVB = "Văn bản test gửi mail";
            ViewBag.MaVB = "88888888";
            ViewBag.NoiDung = "Nội dung chỉnh sửa";
            ViewBag.DoiTuong = "Phòng CNTT";
            ViewBag.HinhThuc = "Khác";
            ViewBag.Link = @"https://vb.bvta.vn/Preview/VanBan?file=5EE2E62A-CE08-4985-9BF1-011972211D85";
            ViewBag.NguoiNhan = "Lương Đức Việt";
            ViewBag.NgayPhatHanh = "27/02/2023";
            ViewBag.NgayHieuLuc = "27/02/2023";
            if(type == 1)
            {
                return PartialView("_TempBieuMau");
            }
            return PartialView("_TempQuyTrinhQuyDinh");
        }

        [HttpPost]
        public async Task<JsonResult> SendMail([FromBody]MailRequest data)
        {
            foreach (var email in data.ListEmail)
            {
                MailRequest request = new MailRequest()
                {
                    Body = data.Body,
                    ToEmail = email,
                    Attachments = data.Attachments,
                    Subject = "Thông báo ban hành Văn bản: Test ban hành văn bản"
                };
                await _services.MailService.SendEmailAsync(request);
            }
            
            return Json(new { status = true });
        }
    }
}
