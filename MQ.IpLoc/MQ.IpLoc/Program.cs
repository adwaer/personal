using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.IpLoc
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var stream = File.OpenRead("geobase.dat"))
            using (var reader = new BinaryReader(stream))
            {
                var header = Header.Get(reader);
                
                var records = reader.ReadInt32();
                var ranges = reader.ReadUInt32();
            }

            Console.ReadLine();
        }
    }
}
