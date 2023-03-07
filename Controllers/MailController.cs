using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;
using WebTools.Models.ViewModel;
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

        [HttpPost]
        public IActionResult MailTemplate([FromBody]List<VanBan_BanHanh> vanban)
        {
            BanHanhVanBanVM model = new BanHanhVanBanVM();
            model.listVanBan = vanban;
            return PartialView("_TempQuyTrinhQuyDinh", model);
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
