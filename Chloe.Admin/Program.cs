using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using NLog.Web;

namespace Chloe.Admin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /*
             * 关键对象：
             *    Microsoft.AspNetCore.Hosting.WebHostBuilder   http://www.cnblogs.com/bill-shooting/p/SourceCode_Hosting.html   http://www.cnblogs.com/artech/p/asp-net-core-real-pipeline-06.html
             *    
             * 1. 创建 Microsoft.AspNetCore.Hosting.WebHostBuilder 对象。
             * 2. 向 Microsoft.AspNetCore.Hosting.WebHostBuilder 对象绑定相关设置，如监听的url和端口、根目录、Startup.cs
             * 3. 调用 WebHostBuilder.Build() 方法创建出一个 IWebHost 对象
             * 4. 调用 IWebHost.Run()
             *    IWebHost 对象内部会处理 Startup 内的 ConfigureServices() 和 Configure() 方法
             * 5. IWebHost 监听请求，将请求交给中间件处理
             */

            /*
             * 中间件介绍：http://www.cnblogs.com/artech/p/asp-net-core-real-pipeline-01.html
             * 关键对象：
             *    Microsoft.AspNetCore.Builder.Internal.ApplicationBuilder
             *    Microsoft.AspNetCore.Hosting.Internal.HostingApplication
             * 
             * 1. 调用IApplicationBuilder.Use(Func<RequestDelegate, RequestDelegate> middleware) 注册中间件。
             *    
             * 2. 调用IApplicationBuilder.Build() 生成一个 RequestDelegate 委托。
             *    中间件并不孤立地存在，所有注册的中间件最终会根据注册的先后顺序组成一个链表，每个中间件不仅仅需要完成各自的请求处理任务外，还需要驱动链表中的下一个中间件；
             *    某个中间件会将后一个Func<RequestDelegate, RequestDelegate>对象的返回值作为输入，而自身的返回值则作为前一个中间件的输入。
             *    某个中间件执行之后返回的RequestDelegate对象不仅仅体现了自身对请求的处理操作，而是体现了包含自己和后续中间件一次对请求的处理。
             *    那么对于第一个中间件来说，它执行后返回的RequestDelegate对象实际上体现了整个应用对请求的处理逻辑。
             *    
             * 3. 得到 RequestDelegate 委托，就可以将 RequestDelegate 对象传入 Microsoft.AspNetCore.Hosting.Internal.HostingApplication。
             * 
             * 4.最后调用 HostingApplication.ProcessRequestAsync(Context context)，就可以处理请求逻辑
             */

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); /* 支持中文 */

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
