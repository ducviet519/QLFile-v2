using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;

namespace WebTools.Services.Interface
{
    public interface IThuMucServices
    {
        Task<List<ThuMuc>> GetList_ThuMuc(int? parentid, string type);

        Task<string> Add_ThuMuc(ThuMuc thumuc);
        Task<string> Rename_ThuMuc(ThuMuc thumuc);
        Task<string> Switch_ThuMuc(ThuMuc thumuc);
    }
}
