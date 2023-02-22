using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;

namespace WebTools.Services.Interface
{
    public interface IGopYServices
    {
        Task<string> ThemGopY(string IDBieuMau, string noidung, string user);

        Task<List<GopY>> GetListGopY(string idvb, string user);
    }
}
