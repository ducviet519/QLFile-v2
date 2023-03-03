using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models.Entities
{
    public class EasySignResponse
    {
        public string status { get; set; }
        public string msg { get; set; }
        public string data { get; set; }
    }

    public class InviciableSignFileResponse
    {
        public int status { get; set; }
        public string msg { get; set; }
        public FileDataResponse data { get; set; }
    }

    public class FileDataResponse
    {
        public List<ResponseContentList> responseContentList { get; set; }
        public string base64Certificate { get; set; }
    }

    public class ResponseContentList
    {
        public string documentName { get; set; }
        public string signatureOnly { get; set; }
        public string signedDocument { get; set; }
    }
}
