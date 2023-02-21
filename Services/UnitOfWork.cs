using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTools.Services;
using WebTools.Services.Interface;

namespace WebTools.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        public IReportListServices Report_List { get; }
        public IReportVersionServices Report_Version { get; }
        public IReportSoftServices Report_Soft { get; }
        public IReportDetailServices Report_Detail { get; }
        public IReportURDServices DM_URDs { get; }
        public IDepts DM_Depts { get; }
        public ISoftwareServices DM_Softwares { get; }
        public IGoogleDriveAPI GoogleDriveAPI { get; }
        public IGoogleDriverV2 GoogleDriveV2 { get; }
        public IUploadFileServices UploadFile { get; }
        public IGopYServices Report_GopY { get; }

        public IDanhMucServices DanhMuc { get; }
        public IVanBanServices VanBan { get; }
        public IThuMucServices ThuMuc { get; }
        public IPhanQuyenServices PhanQuyen { get; }
        public IThongKeServices ThongKe { get; }

        public UnitOfWork
            (
                IReportListServices _Report_List,
                IReportVersionServices _Report_Version,
                IReportSoftServices _Report_Soft,
                IReportDetailServices _Report_Detail,
                IGopYServices _Report_GopY,

                IReportURDServices _DM_URDs,
                IDepts _DM_Depts,
                ISoftwareServices _DM_Softwares,

                IGoogleDriveAPI _GoogleDriveAPI,
                IGoogleDriverV2 _googleDriverV2,
                IUploadFileServices _UploadFile,

                IDanhMucServices _danhMucServices,
                IVanBanServices _vanBanServices,
                IThuMucServices _thuMucServices,
                IPhanQuyenServices _phanQuyenServices,
                IThongKeServices _thongKeServices

            )
        {
            Report_List = _Report_List;
            Report_Version = _Report_Version;
            Report_Soft = _Report_Soft;
            Report_Detail = _Report_Detail;
            Report_GopY = _Report_GopY;

            DM_URDs = _DM_URDs;
            DM_Depts = _DM_Depts;
            DM_Softwares = _DM_Softwares;
            
            GoogleDriveAPI = _GoogleDriveAPI;
            GoogleDriveV2 = _googleDriverV2;
            UploadFile = _UploadFile;

            DanhMuc = _danhMucServices;
            VanBan = _vanBanServices;
            ThuMuc = _thuMucServices;
            PhanQuyen = _phanQuyenServices;
            ThongKe = _thongKeServices;

        }

    }
}
