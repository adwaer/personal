using System;
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
            DateTime start1 = DateTime.Now;

            //using (var stream = File.OpenRead("geobase.dat"))
            //{
            //    WriteLog(ref start, "read");
            //DbReadMethods.ReadByMethods(ref start); // 400 ms
            //DbReadMethods.ReadByQuery(ref start); // 500 ms
            //}
            
            DbReadMethods.ReadUnsafe(); // ??? ms

            WriteLog(ref start1, "spent");
            Console.ReadLine();
        }

        static void WriteLog(ref DateTime start, string action)
        {
            Console.WriteLine($"{action} time: {(DateTime.Now - start).TotalMilliseconds}");
            start = DateTime.Now;
        }
    }
}
