using System;
using System.Configuration;
using Microsoft.Owin.Hosting;
using Bets.WebApi;

namespace Bets.Host
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
