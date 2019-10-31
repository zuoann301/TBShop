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
using Chloe.Admin.Common;

 

namespace Chloe.Admin
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
                      .AddJsonFile(Path.Combine("configs", $"appsettings.{env.EnvironmentName}.json"), optional: true, reloadOnChange: true)
                      .AddEnvironmentVariables();                                              // override settings with environment variables set in compose.   

            //if (env.IsDevelopment())
            //{
            //    // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
            //    builder.AddUserSecrets();
            //}

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
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false; // Default is true, make it false
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            //services.AddSession();
            services.AddDistributedMemoryCache();
            services.AddSession(option =>
            {
                option.IOTimeout = TimeSpan.FromHours(1);
                option.IdleTimeout = TimeSpan.FromHours(1);
                option.Cookie.Name = "wxshop";
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
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            /* NLog */
            this._env.ConfigureNLog(Path.Combine("configs", "nlog.config"));
            loggerFactory.AddNLog();
            //app.AddNLogWeb(); 

            Globals.Services = app.ApplicationServices;
            AceMapper.InitializeMap(); /* 初始化 AutoMapper */

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseCookiePolicy();
            app.UseSession();

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
