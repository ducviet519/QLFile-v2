using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models.Entities
{

    public class FilePDFInviciableRequest
    {
        public List<FilePDFInviciablContents> signingRequestContents { get; set; }
        public TokenInfo tokenInfo { get; set; }
        //public Optional optional { get; set; }
    }

    public class FilePDFInviciablContents
    {
        public string data { get; set; }
        public string documentName { get; set; }
    }

    public class EasySignRequest
    {
        public List<SigningRequestContents> signingRequestContents { get; set; }
        public TokenInfo tokenInfo { get; set; }
    }

    public class SigningRequestContents
    {
        public string data { get; set; }
        public string documentName { get; set; }
        public RequestContentsLocation location { get; set; }
        public RequestContentsExtraInfo extraInfo { get; set; }
        public string imageSignature { get; set; }

    }
    public class RequestContentsLocation
    {
        public int visibleX { get; set; }
        public int visibleY { get; set; }
        public int visibleWidth { get; set; }
        public int visibleHeight { get; set; }
    }
    public class RequestContentsExtraInfo
    {
        public int pageNum { get; set; }
    }

    public class TokenInfo
    {
        public string pin { get; set; }
        public string serial { get; set; }
    }
    public class Optional
    {
        public string otpCode { get; set; }
        public string hashAlgorithm { get; set; }
        public bool returnInputData { get; set; }
        public string signAlgorithm { get; set; }
    }
}
