using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace aspnetapp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddEnvironmentVariables();
                    var a = config.Build();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        //if (File.Exists(certificate))
                        //{
                        //    Console.WriteLine($"Configuring kestrel to bind to certificate'{certificate}' on port 443");
                        //    //var fileContents = File.ReadAllText(certificate);
                        //    //Console.WriteLine(fileContents);
                        //    //serverOptions.Listen(IPAddress.Any, 443, listenOptions =>
                        //    //{
                        //    //    listenOptions.UseHttps(certificate,null, options =>
                        //    //    {
                        //    //        options.AllowAnyClientCertificate();
                        //    //    });
                        //    //});
                        //}
                       

                        //serverOptions.Listen(IPAddress.Any, 8000, listenOptions =>
                        //{
                        //    listenOptions.UseConnectionLogging();
                        //});

                        ////https://docs.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel?view=aspnetcore-3.1
                        //serverOptions.Listen(IPAddress.Any, 443, listenOptions =>
                        //{
                        //    listenOptions.UseHttps("/mnt/secrets-store/ihcaas", "ihcaas");
                        //});
                        ////serverOptions.ConfigureHttpsDefaults(listenOptions =>
                        ////{
                        ////    // certificate is an X509Certificate2
                        ////    //listenOptions.Us = certificate;
                        ////});


                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
