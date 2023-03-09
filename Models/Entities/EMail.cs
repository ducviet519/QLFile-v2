using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models.Entities
{
    public class EMail
    {
        public string emailTo { get; set; }
        public string emailSubject { get; set; }
        public string emailBody { get; set; }
        public List<IFormFile> emailAttachment { get; set; }
    }

    public class MailRequest
    {
        public string ToEmail { get; set; }
        public List<string> ListEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }

    public class MailSettings
    {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }

    public class MailData
    {
        public string IDVanBan { get; set; }
        public string emailBody { get; set; }
        public string emailAttachments { get; set; }
        public string emailSubject { get; set; }
    }

    public class MailAccount
    {
        public string AccountName { get; set; }
        public string Password { get; set; }
    }
}
