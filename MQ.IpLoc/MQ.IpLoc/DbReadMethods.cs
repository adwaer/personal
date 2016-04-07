using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using MQ.Business;
using MQ.Cqrs.Factory;
using MQ.Cqrs.Impl;
using MQ.Domain;

namespace MQ.IpLoc
{
    internal class DbReadMethods
    {
        private const string FilePath = "geobase.dat";
        public static void ReadByQuery(ref DateTime start)
        {
            Helpers.WriteLog(ref start, "read");

            using (var stream = File.OpenRead(FilePath))
            {
                Header header;
                using (BinaryReader reader = new BinaryReader(Helpers.CloneStream(stream, 60)))
                {
                    header = Header.Get(reader);
                }
                
                using (BinaryReader reader = new BinaryReader(Helpers.CloneStream(stream, 20 * header.RecordCount)))
                {
                    var ipLocations = new IpLocationQuery()
                        .Execute(new ManagedReader(reader), header.RecordCount);
                }
                
                using (BinaryReader reader = new BinaryReader(Helpers.CloneStream(stream, 96 * header.RecordCount)))
                {
                    var locations = new LocationQuery()
                        .Execute(new ManagedReader(reader), header.RecordCount);
                }
                
                using (BinaryReader reader = new BinaryReader(Helpers.CloneStream(stream, 4 * header.RecordCount)))
                {
                    var indexes = new IndexQuery()
                        .Execute(new ManagedReader(reader), header.RecordCount);
                }
            }
        }

        public static void ReadByMethods(ref DateTime start)
        {
            Helpers.WriteLog(ref start, "read");
            using (var stream = File.OpenRead(FilePath))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    IBinaryReader binaryReader = new ManagedReader(reader);

                    var header = GetHeader(binaryReader);
                    var ipLocations = GetIpLocation(binaryReader, header.RecordCount);
                    var locations = GetLocation(binaryReader, header.RecordCount);
                    var indexes = GetIndexes(binaryReader, header.RecordCount);
                }
            }
        }

        public static void ReadUnsafe(ref DateTime start)
        {
            Helpers.WriteLog(ref start, "read");
            MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(FilePath);
            MemoryMappedViewStream stream = mmf.CreateViewStream();

            IBinaryReader binaryReader = new UnmanagedReader(stream);

            var header = GetHeader(binaryReader);
            var ipLocations = GetIpLocation(binaryReader, header.RecordCount);
            var locations = GetLocation(binaryReader, header.RecordCount);
            var indexes = GetIndexes(binaryReader, header.RecordCount);
        }

        private static Header GetHeader(IBinaryReader reader)
        {
            var factory = new HeaderFactory();
            return factory.Get(reader);
        }
        private static IpLocation[] GetIpLocation(IBinaryReader reader, int recordCount)
        {
            IpLocationFactory factory = new IpLocationFactory();

            IpLocation[] ipLocations = new IpLocation[recordCount];
            for (uint i = 0; i < recordCount; i++)
            {
                ipLocations[i] = factory.Get(reader);
            }

            return ipLocations;
        }
        private static Location[] GetLocation(IBinaryReader reader, int recordCount)
        {
            LocationFactory factory = new LocationFactory();

            Location[] locations = new Location[recordCount];
            for (int i = 0; i < recordCount; i++)
            {
                locations[i] = factory.Get(reader);
            }

            return locations;
        }
        private static float[] GetIndexes(IBinaryReader reader, int recordCount)
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
