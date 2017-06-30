using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace monolithic_shop_core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var portNumber = args[0];

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseUrls($"http://*:{portNumber}")
                .Build();

            host.Run();
        }
    }
}
