using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace BackendApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
                         var config = new ConfigurationBuilder()                    
                        .SetBasePath(new DirectoryInfo(Directory.GetCurrentDirectory()).FullName + "/config")  
                        .AddJsonFile("config.json", optional: false)
                        .Build(); 
                        // Setup a HTTP/2 endpoint without TLS.
                        options.ListenLocalhost(config.GetValue<int>("Api:port"), o => o.Protocols =
                                    HttpProtocols.Http2);
                    });

                    webBuilder.UseStartup<Startup>();
                });

    }
}
