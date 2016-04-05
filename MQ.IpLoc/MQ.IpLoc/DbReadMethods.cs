using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQ.Cqrs.Impl;
using MQ.Domain;

namespace MQ.IpLoc
{
    internal class DbReadMethods
    {
        public static void ReadByQuery(ref DateTime start)
        {
            Helpers.WriteLog(ref start, "read");

            using (var stream = File.OpenRead("geobase.dat"))
            {
                Header header;
                using (BinaryReader reader = new BinaryReader(Helpers.CloneStream(stream, 60)))
                {
                    header = Header.Get(reader);
                }

                var ipLocations = new IpLocationQuery()
                    .Execute(Helpers.CloneStream(stream, 20*header.RecordCount), header.RecordCount);

                var locations = new LocationQuery()
                    .Execute(Helpers.CloneStream(stream, 96*header.RecordCount), header.RecordCount);

                var indexes = new IndexQuery()
                    .Execute(Helpers.CloneStream(stream, 4*header.RecordCount), header.RecordCount);

                Console.WriteLine($"last ip loc: {ipLocations.Result[header.RecordCount - 1]}");
                Console.WriteLine($"last loc: {locations.Result[header.RecordCount - 1]}");
                Console.WriteLine($"last index: {indexes.Result[header.RecordCount - 1]}");
            }
        }
        public static void ReadByMethods(ref DateTime start)
        {
            Helpers.WriteLog(ref start, "read");
            using (var stream = File.OpenRead("geobase.dat"))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    Header header = Header.Get(reader);
                    var ipLocations = GetIpLocation(reader, header.RecordCount);
                    var locations = GetLocation(reader, header.RecordCount);
                    var indexes = GetIndexes(reader, header.RecordCount);

                    Console.WriteLine($"last ip loc: {ipLocations[header.RecordCount - 1]}");
                    Console.WriteLine($"last loc: {locations[header.RecordCount - 1]}");
                    Console.WriteLine($"last index: {indexes[header.RecordCount - 1]}");
                }
            }
        }

        public static void ReadUnsafe(ref DateTime start)
        {
            using(var stream = new UnmanagedMemoryStream())
        }

        private static IpLocation[] GetIpLocation(BinaryReader reader, int recordCount)
        {
            IpLocation[] ipLocations = new IpLocation[recordCount];
            for (uint i = 0; i < recordCount; i++)
            {
                ipLocations[i] = IpLocation.Get(reader);
            }

            return ipLocations;
        }
        private static Location[] GetLocation(BinaryReader reader, int recordCount)
        {
            Location[] locations = new Location[recordCount];
            for (int i = 0; i < recordCount; i++)
            {
                locations[i] = Location.Get(reader);
            }

            return locations;
        }
        private static float[] GetIndexes(BinaryReader reader, int recordCount)
        {
            float[] indexes = new float[recordCount];
            for (int i = 0; i < recordCount; i++)
            {
                indexes[i] = reader.ReadInt32();
            }

            return indexes;
        }
    }
}
