using System;
using System.CodeDom;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MQ.Cqrs.Impl;
using MQ.Domain;

namespace MQ.IpLoc
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            DateTime start1 = DateTime.Now;
            DateTime start = DateTime.Now;

            //using (var stream = File.OpenRead("geobase.dat"))
            //{
            //    Helpers.WriteLog(ref start, "read");
                //DbReadMethods.ReadByMethods(ref start); // 400 ms
                //DbReadMethods.ReadByQuery(ref start); // 500 ms
            //}
            DbReadMethods.ReadUnsafe(ref start); // ??? ms


            Helpers.WriteLog(ref start1, "spent");
            Console.ReadLine();
        }

    }
}
