using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using Microsoft.Owin.Hosting;
using MQ.Business;
using MQ.Dal;
using MQ.WebApi;

namespace MQ.Host
{
    class Program
    {
        private static void Main()
        {
            var watch = new Stopwatch();
#if !DEBUG
            StartDelay(5);
#endif  

            watch.Start();

            Singleton<EntityDataSet>.Instance
                .Fetch();

            watch.Stop();

            WriteLog(watch.ElapsedMilliseconds, "spent");

            var host = ConfigurationManager.AppSettings["host"];
            using (WebApp.Start<Startup>(host))
            {

                Console.WriteLine($"Starting host for uri: {host}");
                Console.WriteLine();
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();

            }
        }

        static void WriteLog(double spent, string action)
        {
            Console.WriteLine($"{action} time: {spent}ms");
        }

        static void StartDelay(int seconds)
        {
            Console.WriteLine("Starting in  ");
            for (int i = seconds; i > 0; i--)
            {
                Console.SetCursorPosition(12, 0);
                Console.Write($"{i}..");
                Thread.Sleep(1000);
            }

            Console.SetCursorPosition(12, 0);
            Console.Write(".. started!");
            Console.WriteLine();
        }
    }

}
