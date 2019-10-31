using Ace;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.AspNetCore.Hosting
{
    public static class WebHostBuilderListenerExtension
    {
        public static IWebHostBuilder BindUrls(this IWebHostBuilder hostBuilder, string[] appLaunchArgs, string contentRoot)
        {
            /*
             * 1.从启动参数中传入要监听的 useurls，格式如：dotnet run -useurls http://localhost:5001;http://localhost:5002
             * 2.如果启动参数中未包含 useurls 参数，则从 config/hosting.json 配置中查找要监听的 useurls
             * ！如果通过上述两个方式都找不到 urls，则使用默认方式！
             */

            Console.WriteLine(string.Format("input start args: {0}", JsonHelper.Serialize(appLaunchArgs)));

            string[] urls = new string[0];
            List<string> inputArgs = appLaunchArgs == null ? new List<string>() : appLaunchArgs.ToList();
            int indexOf_urls = inputArgs.IndexOf("-useurls");
            if (indexOf_urls != -1 && inputArgs.Count > indexOf_urls + 1)
            {
                urls = inputArgs[indexOf_urls + 1].Split(';');
            }
            else
            {
                string hostingFile = Path.Combine(contentRoot, "configs", "hosting.json");
                if (File.Exists(hostingFile))
                {
                    Console.WriteLine(string.Format("hosting.json exists: {0}", hostingFile));

                    var hostingConfig = new ConfigurationBuilder()
                                .SetBasePath(contentRoot)
                                .AddJsonFile(hostingFile, true)
                                .Build();
                    string urlsValue = hostingConfig["urls"];
                    if (string.IsNullOrEmpty(urlsValue) == false)
                    {
                        urls = urlsValue.Split(';');
                    }
                }
            }

            if (urls.Length >= 0)
            {
                hostBuilder = hostBuilder.UseUrls(urls);
            }

            return hostBuilder;
        }

    }
}
