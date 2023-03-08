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
        Task<List<ThongKe_BaoCaoDocHieuChiTiet>> GetData_BaoCaoDocHieuChiTiet(string idvb);
        Task<List<ThongKe_PhamViVB>> GetData_ThongKePhamVi(SearchThongKe search);
        Task<List<ThongKe_TongHop>> GetData_ThongKeTongHop(SearchThongKe search);

    }
}
