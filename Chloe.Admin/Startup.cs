//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;
//using Ace;
//using Ace.Application;
//using Ace.AutoMapper;
//using Ace.Web.Mvc;
//using Chloe.ES.Common;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ace.Application;
using Microsoft.Extensions.Configuration;
using Ace;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using System.IO;
using Microsoft.Extensions.DependencyModel;
using Ace.AutoMapper;
using System.Text;
using Ace.Web.Mvc;
using NLog.Extensions.Logging;
using NLog.Web;
using Microsoft.Extensions.Caching.Memory;

using Microsoft.AspNetCore.Mvc;
using Senparc.CO2NET.RegisterServices;
using Senparc.Weixin.RegisterServices;
using Microsoft.Extensions.Options;
using Senparc.CO2NET;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.WxOpen;
using Senparc.Weixin.TenPay;
using Chloe.Admin.Common;

namespace Chloe.API
{
    public class Startup
    {
        IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            this._env = env;

            var builder = new ConfigurationBuilder()
                      .SetBasePath(env.ContentRootPath)
                      .AddJsonFile(Path.Combine("configs", "appsettings.json"), optional: true, reloadOnChange: true)  // Settings for the application
                      //.AddJsonFile(Path.Combine("configs", $"appsettings.{env.EnvironmentName}.json"), optional: true, reloadOnChange: true)
                      .AddEnvironmentVariables();                                              // override settings with environment variables set in compose.   

           
            Configuration = builder.Build();
            Globals.Configuration = Configuration;
            Globals.AppRootPath = _env.ContentRootPath;
        }

        public IConfigurationRoot Configuration { get; }



        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(this.Configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDatabase(this.Configuration);
            services.RegisterAppServices(); /* 注册应用服务 */

            /* 缓存 */
            services.AddMemoryCache();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddDistributedMemoryCache();
            services.AddSession(option =>
            {
                option.IOTimeout = TimeSpan.FromHours(1);
                option.IdleTimeout = TimeSpan.FromHours(1);
                option.Cookie.Name = "zxcycn";
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
            });

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                options.Filters.Add(typeof(WebPermissionFilter));
            }).AddControllersAsServices();

            services.AddSenparcGlobalServices(Configuration)//Senparc.CO2NET 全局注册
                    .AddSenparcWeixinServices(Configuration);//Senparc.Weixin 注册
        }

       

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<SenparcSetting> senparcSetting, IOptions<SenparcWeixinSetting> senparcWeixinSetting)
        {
            this._env.ConfigureNLog(Path.Combine("configs", "nlog.config"));
            loggerFactory.AddNLog();

            Globals.Services = app.ApplicationServices;
            AceMapper.InitializeMap();

            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseCookiePolicy();
            app.UseSession();

            IRegisterService register = RegisterService.Start(env, senparcSetting.Value).UseSenparcGlobal();
            register.UseSenparcWeixin(senparcWeixinSetting.Value, senparcSetting.Value)
                .RegisterWxOpenAccount(senparcWeixinSetting.Value, "【TMS】小程序")
                .RegisterTenpayV3(senparcWeixinSetting.Value, "【TMS】公众号");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "area",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
