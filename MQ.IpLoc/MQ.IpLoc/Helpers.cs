using System;
using System.IO;

namespace MQ.IpLoc
{
    public class Helpers
    {
        public static void WriteLog(ref DateTime start, string action)
        {
            Console.WriteLine($"{action} time: {(DateTime.Now - start).TotalMilliseconds}");
            start = DateTime.Now;
        }
    }
}
