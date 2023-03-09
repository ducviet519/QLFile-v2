using Microsoft.Extensions.DependencyInjection;
using RestSharp.Authenticators;
using WebTools.Services;
using WebTools.Services.Interface;
using WebTools.Services.Repositories;

namespace WebTools.Services
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IReportListServices, ReportListServices>();
            services.AddTransient<IReportVersionServices, ReportVersionServices>();
            services.AddTransient<IReportSoftServices, ReportSoftServices>();
            services.AddTransient<IReportDetailServices, ReportDetailServices>();
            services.AddTransient<IReportURDServices, ReportURDServices>();
            services.AddTransient<IDepts, DeptsServices>();
            services.AddTransient<ISoftwareServices, SoftwareServices>();
            services.AddTransient<IGoogleDriveAPI, GoogleDriveAPI>();
            services.AddTransient<IGoogleDriverV2, GoogleDriverV2>();
            services.AddTransient<IUploadFileServices, UploadFileServices>();
            services.AddTransient<IGopYServices, GopYServices>();
            services.AddTransient<IDanhMucServices, DanhMucServices>();
            services.AddTransient<IVanBanServices, VanBanServices>();
            services.AddTransient<IThuMucServices, ThuMucServices>();
            services.AddTransient<IPhanQuyenServices, PhanQuyenServices>();
            services.AddTransient<IThongKeServices, ThongKeServices>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IEasySignServices, EasySignServices>();
            services.AddTransient<IReportsAPIServices, ReportsAPIServices>();
        }
    }
}
