using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using Microsoft.Owin.Hosting;
using MQ.Dal;
using MQ.WebApi;

namespace MQ.Host
{
    public class Program
    {
        private static void Main()
        {
            FetchData();

            var host = ConfigurationManager.AppSettings["host"];
            using (WebApp.Start<Startup>(host))
            {

                Console.WriteLine($"Starting host for uri: {host}");
                Console.WriteLine();

                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }

        private static void FetchData()
        {
            var watch = new Stopwatch();
#if !DEBUG
            StartDelay(5);
#endif

            watch.Start();

            EntityDataSet.Instance.Fetch();

            watch.Stop();

            WriteLog(watch.ElapsedMilliseconds, "spent");
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
