using WebTools.Services;
using WebTools.Services.Interface;

namespace WebTools.Services
{
    public interface IUnitOfWork
    {
        IReportListServices Report_List {get;} 
        IReportVersionServices Report_Version { get; }
        IReportSoftServices Report_Soft { get; }
        IReportDetailServices Report_Detail { get; }
        IGopYServices Report_GopY { get; }

        IReportURDServices DM_URDs { get; }
        IDepts DM_Depts { get; }
        ISoftwareServices DM_Softwares { get; }

        IGoogleDriveAPI GoogleDriveAPI { get; }
        IGoogleDriverV2 GoogleDriveV2 { get; }
        IUploadFileServices UploadFile { get; }

        IDanhMucServices DanhMuc { get; }
        IVanBanServices VanBan { get; }
        IThuMucServices ThuMuc { get; }
        IPhanQuyenServices PhanQuyen { get;}
        IThongKeServices ThongKe { get; }
    }
}
