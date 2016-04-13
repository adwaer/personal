using System;
using System.Diagnostics;
using System.Threading;
using MQ.Business;
using MQ.Dal;

namespace MQ.IpLoc
{
    internal class Program
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

            Console.WriteLine("Press any key to exit");
            Console.WriteLine();
            Console.ReadKey();
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
