using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QueryFilterServiceApp.Data;

namespace QueryFilterServiceApp {
    public class Program {
        public static void Main(string[] args) {
            var host = CreateWebHostBuilder(args).Build();
            CreateDbIfNotExists(host);
            host.Run();
        }

        static void CreateDbIfNotExists(IWebHost host) {
            using(var scope = host.Services.CreateScope()) {
                var context = scope.ServiceProvider.GetRequiredService<SchoolContext>();
                DbInitializer.Initialize(context);
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        }
    }
}
