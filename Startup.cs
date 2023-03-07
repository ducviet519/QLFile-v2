using DevExpress.AspNetCore;
using GleamTech.AspNet.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;
using WebTools.Authorization;
using WebTools.Context;
using WebTools.Services;
using WebTools.Services.Interface;

namespace WebTools
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddControllersWithViews();
            services.AddAuthentication(           
                CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/User/Login";
                    options.AccessDeniedPath = "/User/Denied";
                    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.IsEssential = true;
                    options.Events = new CookieAuthenticationEvents()
                    {
                        OnSigningIn = async context =>
                        {
                            await Task.CompletedTask;
                        },
                        OnSignedIn = async context =>
                        {
                            await Task.CompletedTask;
                        },
                        OnValidatePrincipal = async context =>
                        {
                            await Task.CompletedTask;
                        }
                    };
                });
            services.AddSession(options =>
            {
		        options.IdleTimeout = TimeSpan.FromDays(1);
                //options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
                //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                //options.Cookie.IsEssential = true;
            });
            services.AddGleamTech();
            services.AddDevExpressControls();
            services.AddMvc();
            services.AddScoped<IReportListServices, ReportListServices>();
            services.AddScoped<IReportVersionServices, ReportVersionServices>();
            services.AddScoped<IReportSoftServices, ReportSoftServices>();
            services.AddScoped<IReportDetailServices, ReportDetailServices>();
            services.AddScoped<IReportURDServices, ReportURDServices>();
            services.AddScoped<IDepts, DeptsServices>();
            services.AddScoped<ISoftwareServices, SoftwareServices>();
            services.AddScoped<IGoogleDriveAPI, GoogleDriveAPI>();
            services.AddScoped<IUploadFileServices, UploadFileServices>();
            services.AddScoped<IGopYServices, GopYServices>();           
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ToolsDB")));
            services.AddMvc().AddRazorPagesOptions(o => {
                o.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
            });
            services.AddInfrastructure();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDevExpressControls();
            System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls12;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //Register GleamTech to the ASP.NET Core HTTP request pipeline.
            //----------------------
            app.UseGleamTech();
            //----------------------
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "node_modules")),
                RequestPath = "/node_modules"
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
          
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });           
        }
    }
}
