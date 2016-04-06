using System;
using System.IO;
using System.Text;

namespace MQ.Domain
{
    public class EnvironmentConstant
    {
        public const char EmptySpace = '\0';
        public static DateTime UnixDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static string ReadString(BinaryReader reader, int count)
        {
            return Encoding.Default
                .GetString(reader.ReadBytes(count))
                .TrimEnd(EmptySpace);
        }
        
    }
}
