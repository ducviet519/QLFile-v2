using DevExpress.AspNetCore.Reporting.QueryBuilder;
using DevExpress.AspNetCore.Reporting.QueryBuilder.Native.Services;
using DevExpress.AspNetCore.Reporting.ReportDesigner;
using DevExpress.AspNetCore.Reporting.ReportDesigner.Native.Services;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer.Native.Services;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.Web.WebDocumentViewer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;
using WebTools.Reports;
using WebTools.Services;

namespace WebTools.Controllers
{
    #region ReportingControllers
    public class CustomWebDocumentViewerController : WebDocumentViewerController
    {
        public CustomWebDocumentViewerController(IWebDocumentViewerMvcControllerService controllerService) : base(controllerService)
        {
        }
    }
    public class CustomReportDesignerController : ReportDesignerController
    {
        public CustomReportDesignerController(IReportDesignerMvcControllerService controllerService) : base(controllerService)
        {
        }
    }
    public class CustomQueryBuilderController : QueryBuilderController
    {
        public CustomQueryBuilderController(IQueryBuilderMvcControllerService controllerService) : base(controllerService)
        {
        }
    }
    #endregion

    public class ReportingController: Controller
    {
        #region Services
        private readonly IUnitOfWork _services;
        public ReportingController(IUnitOfWork services)
        {
            _services = services;
        }
        #endregion
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Designer()
        {
            ReportDesigner model = new ReportDesigner();
            return View(model);
        }

        public async Task<object> DeptsDataSource()
        {
            ObjectDataSource dataSource = new ObjectDataSource();
            dataSource.Name = "DeptsObject";
            dataSource.DataSource = await _services.DanhMuc.Get_DM_Depts();
            return dataSource;
        }
    }
}
