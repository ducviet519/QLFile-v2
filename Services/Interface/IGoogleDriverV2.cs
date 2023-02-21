using Google.Apis.Drive.v2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Services.Interface
{
    public interface IGoogleDriverV2
    {
        Task<IList<Comment>> GetDriverComments(string fileId);
    }
}
