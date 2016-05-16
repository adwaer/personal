using System;
using System.Configuration;
using Microsoft.Owin.Hosting;
using SN.WebApi;

namespace SN.Host
{
    public class Program
    {
        private static void Main()
        {
            var host = ConfigurationManager.AppSettings["host"];
            using (WebApp.Start<Startup>(host))
            {

                Console.WriteLine($"Starting host for uri: {host}");
                Console.WriteLine();

                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }

    }

}
