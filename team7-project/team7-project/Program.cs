using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace team7_project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Environment.SetEnvironmentVariable("CUSTOMCONNSTR_mongoDB", "mongodb+srv://admin:cdznjqbcnjxybr@clusterteam7-hb7ef.azure.mongodb.net/test?retryWrites=true");
            Environment.SetEnvironmentVariable("AUTH_KEY", "sdfsdfsdfsdfaghh");

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
