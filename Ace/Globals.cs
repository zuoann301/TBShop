using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ace
{
    public static class Globals
    {
        public static IServiceProvider Services { get; set; }
        public static IConfigurationRoot Configuration { get; set; }


        public static string AppRootPath { get; set; }

        public static string CacheShop = "CacheShop";
        public static string CacheShop2 = "CacheShop2";

        public static string ShopID = string.Empty;
        public static string ShopUrl = string.Empty;

        public static string SmsCode = "sms_code";
    }
}
