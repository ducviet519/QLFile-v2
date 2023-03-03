using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;

namespace WebTools.Services.Interface
{
    public interface IEasySignServices
    {
        Task<InviciableSignFileResponse> InvisiableSignaturePDF(List<FilePDFInviciablContents> fileData);
        Task<EasySignResponse> DigitalSignaturePDF(List<SigningRequestContents> fileData);
        Task<EasySignResponse> GetImageSignature();
    }
}
