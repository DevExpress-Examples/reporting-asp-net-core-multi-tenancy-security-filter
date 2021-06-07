using System.Data.Common;
using DevExpress.AspNetCore;
using DevExpress.AspNetCore.Reporting;
using DevExpress.DataAccess.Web;
using DevExpress.XtraReports.Web.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QueryFilterServiceApp.Data;
using QueryFilterServiceApp.Services;

namespace QueryFilterServiceApp {
    public class Startup {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment) {
            Configuration = configuration;
            Env = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

            services.AddDevExpressControls();
            services.AddDbContext<SchoolContext>(options =>
                options.UseSqlServer(RemoveXpoProviderKey(Configuration.GetConnectionString("QueryFilterServiceApp")))
            );

            var builder = services
                .AddMvc()
                .RemoveDefaultReportingControllers()    // NOTE: make sure the default document viewer controller is not registered
                .AddNewtonsoftJson()
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);
#if DEBUG
            if(Env.IsDevelopment()) {
                builder.AddRazorRuntimeCompilation();
            }
#endif

            services.ConfigureReportingServices(configurator => {
                configurator.ConfigureReportDesigner(designerConfigurator => {
                    designerConfigurator.RegisterDataSourceWizardConfigFileConnectionStringsProvider();
                });
                configurator.ConfigureWebDocumentViewer(viewerConfigurator => {
                    viewerConfigurator.UseCachedReportSourceBuilder();
                });
            });

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ReportStorageWebExtension, ReportStorage>();
            services.AddScoped<ISelectQueryFilterService, SelectQueryFilterService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory) {
            app.UseDevExpressControls();
            if(env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        static string RemoveXpoProviderKey(string connectionString) {
            const string xpoProviderKey = "XpoProvider";

            var builder = new DbConnectionStringBuilder() { ConnectionString = connectionString };
            builder.Remove(xpoProviderKey);

            return builder.ToString();
        }
    }
}
