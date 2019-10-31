using Ace;
using Ace.Data;
using Chloe;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfigurationRoot config)
        {
            string connString = Globals.Configuration["db:ConnString"];
            string dbType = Globals.Configuration["db:DbType"];

            IDbContextFactory dbContextFactory = new DefaultDbContextFactory(dbType, connString);
            services.AddSingleton<IDbContextFactory>(dbContextFactory);
            services.AddScoped<IDbContext>(a =>
            {
                return a.GetService<IDbContextFactory>().CreateContext();
            });

            return services;
        }
    }
}
