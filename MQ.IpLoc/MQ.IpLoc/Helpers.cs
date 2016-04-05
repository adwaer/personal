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

        public static Stream CloneStream(Stream input, int count)
        {
            var output = new MemoryStream();
            byte[] buffer = new byte[count];
            input.Read(buffer, 0, buffer.Length);

            output.Write(buffer, 0, count);
            output.Seek(0, SeekOrigin.Begin);

            return output;
        }
    }
}
