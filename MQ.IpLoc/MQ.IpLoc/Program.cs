using System;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using MQ.Business;
using MQ.Cqrs.Factory;
using MQ.Domain;

namespace MQ.IpLoc
{
    internal class Program
    {
        private const string FilePath = "geobase.dat";
        private static void Main(string[] args)
        {
            var watch = new Stopwatch();
            watch.Start();

            //using (var stream = File.OpenRead("geobase.dat"))
            //{
            //    WriteLog(ref start, "read");
            //DbReadMethods.ReadByMethods(ref start); // 400 ms
            //DbReadMethods.ReadByQuery(ref start); // 500 ms
            //}


            //DbReadMethods.ReadAsync(); // 400 ms
            DbReadMethods.ReadUnsafeToJson(); // ??? ms

            WriteLog(watch.Elapsed, "spent");
            watch.Stop();
            Console.ReadLine();
        }

        static void WriteLog(TimeSpan spent, string action)
        {
            Console.WriteLine($"{action} time: {spent}");
        }

    }
}
