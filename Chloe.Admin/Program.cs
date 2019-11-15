using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Chloe.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateWebHostBuilder(args).Build().Run();
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); /* 支持中文 */

            string contentRoot = Directory.GetCurrentDirectory();

            var hostBuilder = WebHost.CreateDefaultBuilder(args);
            var host = hostBuilder
                .BindUrls(args, contentRoot)
                .UseContentRoot(contentRoot)
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseNLog()
                .UseStartup<Startup>();
    }
}
