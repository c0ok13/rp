using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
namespace PostForm
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var config = new ConfigurationBuilder()                    
                        .SetBasePath(new DirectoryInfo(Directory.GetCurrentDirectory()).FullName + "/config")  
                        .AddJsonFile("config.json", optional: false)
                        .Build(); 

                    webBuilder.UseUrls($"http://*:{config.GetValue<int>("Service:port")}");
                    webBuilder.UseStartup<Startup>();
                });
    }
}
