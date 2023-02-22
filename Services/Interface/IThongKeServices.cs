using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;

namespace WebTools.Services.Interface
{
    public interface IThongKeServices
    {
        Task<List<ThongKe_BieuDo>> Get_DataBieuDo();
        Task<List<ThongKe_BaoCaoDocHieu>> GetData_BaoCaoDocHieu(SearchThongKe search);
    }
}
