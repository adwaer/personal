using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading.Tasks;
using MQ.Business;
using MQ.Cqrs.Factory;
using MQ.Domain;

namespace MQ.IpLoc
{
    internal class DbReadMethods
    {
        private const string FilePath = "geobase.dat";

        public static async void ReadAsync()
        {
            DateTime start = DateTime.Now;

            MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(FilePath);
            MemoryMappedViewStream stream = mmf.CreateViewStream();

            Helpers.WriteLog(ref start, "stream");

            IBinaryReader binaryReader = new AsyncReader(stream);

            Helpers.WriteLog(ref start, "reader");

            // header
            var header = await new HeaderFactory()
                .Get(binaryReader);
            Helpers.WriteLog(ref start, "header");

            // ip locations
            IpLocationFactory factory = new IpLocationFactory();
            var ipLocations = new IpLocation[header.RecordCount];
            for (uint i = 0; i < header.RecordCount; i++)
            {
                ipLocations[i] = await factory.Get(binaryReader);
            }
            Helpers.WriteLog(ref start, "ip locations");

            LocationFactory locationFactory = new LocationFactory();

            Location[] locations = new Location[header.RecordCount];
            for (int i = 0; i < header.RecordCount; i++)
            {
                locations[i] = await locationFactory.Get(binaryReader);
            }
            Helpers.WriteLog(ref start, "locations");

            float[] indexes = new float[header.RecordCount];
            for (int i = 0; i < header.RecordCount; i++)
            {
                indexes[i] = await binaryReader.ReadInt32();
            }
            Helpers.WriteLog(ref start, "indexes");
        }

        public static void ReadUnsafe()
        {
            DateTime start = DateTime.Now;

            var reader = new ByteBinaryReader(FilePath);
            Helpers.WriteLog(ref start, "stream");

            // header
            var header = new Header
            {
                Version = reader.ReadInt32(),
                Name = reader.ReadString(32),
                MakeTime = reader.ReadDateTime(),
                RecordCount = reader.ReadInt32(),
                RangeOffset = reader.ReadUInt32(),
                CityOffset = reader.ReadUInt32(),
                LocationOffset = reader.ReadUInt32()
            };
            Helpers.WriteLog(ref start, "header");

            // ip locations
            var ipLocations = new IpLocation[header.RecordCount];
            for (uint i = 0; i < header.RecordCount; i++)
            {
                var v = new
                {
                    FromIp = reader.ReadUInt64(),
                    ToIp = reader.ReadUInt64(),
                    Index = reader.ReadUInt32()
                };
            }
            Helpers.WriteLog(ref start, "ip locations");

            Location[] locations = new Location[header.RecordCount];
            for (int i = 0; i < header.RecordCount; i++)
            {
                var v = new
                {
                    Country = reader.ReadString(8),
                    Region = reader.ReadString(12),
                    Postal = reader.ReadString(12),
                    City = reader.ReadString(24),
                    Company = reader.ReadString(32),
                    Lat = reader.ReadFloat(),
                    Lon = reader.ReadFloat()
                };
            }
            Helpers.WriteLog(ref start, "locations");

            float[] indexes = new float[header.RecordCount];
            for (int i = 0; i < header.RecordCount; i++)
            {
                indexes[i] = reader.ReadInt32();
            }
            Helpers.WriteLog(ref start, "indexes");
        }

        public static void ReadUnsafeToJson()
        {
            DateTime start = DateTime.Now;

            var reader = new ByteBinaryReader(FilePath);
            Helpers.WriteLog(ref start, "stream");


            var sb = new StringBuilder();

            int recordCount;
            sb.Append($@"header: 
version: { reader.ReadInt32() }
name: { reader.ReadString(32) }
make_time: { reader.ReadDateTime() }
count: { recordCount = reader.ReadInt32() }
offset_range: { reader.ReadUInt32() }
offset_city: { reader.ReadUInt32() }
offset_location: { reader.ReadUInt32() }
");

            Helpers.WriteLog(ref start, "header");

            sb.Append("ip_locations: [");

            // ip locations
            for (uint i = 0; i < recordCount; i++)
            {
                sb.Append($@" 
from_ip: {reader.ReadUInt64()}
to_ip: {reader.ReadUInt64()}
index: {reader.ReadUInt32()}
");
            }
            sb.Append("]");

            sb.Append("locations: [");

            Helpers.WriteLog(ref start, "ip locations");

            var position = reader.Position;
            Location[] locations = new Location[recordCount];
            Parallel.For(0, recordCount, i =>
            {
                var location = locations[i];

                //location.Fill(
                reader.ReadString(8, position + (i * 96));
                reader.ReadString(12, position + (i * 96) + 8);
                reader.ReadString(12, position + (i * 96) + 20);
                reader.ReadString(24, position + (i * 96) + 32);
                reader.ReadString(32, position + (i * 96) + 56);
                reader.ReadFloat(position + (i * 96) + 88);
                reader.ReadFloat(position + (i * 96) + 92);
                //);
            });

            reader.Position += recordCount * 96;
            Helpers.WriteLog(ref start, "locations");

            float[] indexes = new float[recordCount];
            for (int i = 0; i < recordCount; i++)
            {
                indexes[i] = reader.ReadInt32();
            }
            Helpers.WriteLog(ref start, "indexes");
        }
    }
}
