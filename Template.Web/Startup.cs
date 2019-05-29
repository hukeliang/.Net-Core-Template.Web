using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

using Template.Entity;
using Common.Helper.Core.IOExpand;
using Common.Helper.Core.IOExpand.Implementation;

using Common.Helper.Core.EmailExpand;

namespace Template.Web
{
    public class Startup
    {
        public readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;


        }

        // 此方法由运行时调用。使用此方法将服务添加到容器中。
        // 有关如何配置应用程序的更多信息，请访问 https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //注册数据库服务
            services.AddEntityFrameworkSqlServer().AddDbContext<TemplateDbContext>(options =>
            {
                options.UseSqlServer(Configuration["Connection:DevConnectionStrings"]);
                options.UseCreateSlave(Configuration["Connection:SlaveConnectionStrings1"], Configuration["Connection:SlaveConnectionStrings2"], Configuration["Connection:SlaveConnectionStrings3"]);
            });

            //最大上载限制 （MB）    
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = int.Parse(Configuration["MultipartBodyLengthLimit"]) * 1024 * 1024;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


            #region 注册其他服务

            services.AddHttpContextAccessor();

            services.AddTransient<ISpeedLimit, SpeedLimitService>();
            services.AddTransient<IExcel, ExcelService>();


            services.AddTransient<IVerifyCode, VerifyCodeService>();

            services.AddSendEmail(options =>
            {
                options.MailTitel = Configuration["Email:MailTitel"];
                options.MailSmtp = Configuration["Email:MailSmtp"];
                options.MailAccount = Configuration["Email:MailAccount"];
                options.MailPassWord = Configuration["Email:MailPassWord"];
            });

            #endregion

        }

        // 此方法由运行时调用。使用此方法配置HTTP请求管道。
        // 更多参数 IHttpContextFactory httpContextFactory, DiagnosticSource diagnosticSource, DiagnosticListener diagnosticListener
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCreateDb<TemplateDbContext>();
            //提供对静态资源的访问
            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}
