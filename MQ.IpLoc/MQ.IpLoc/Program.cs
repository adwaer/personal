using System;
using System.Collections.Generic;
using System.IO;
using MQ.Domain;

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

                IpLocation[] ipLocations = new IpLocation[header.RecordCount];
                for (uint i = 0; i < header.RecordCount; i++)
                {
                    ipLocations[i] = IpLocation.Get(reader);
                }

                Location[] locations = new Location[header.RecordCount];
                for (int i = 0; i < header.RecordCount; i++)
                {
                    locations[i] = Location.Get(reader);
                }
            }

            Console.ReadLine();
        }
    }
}
