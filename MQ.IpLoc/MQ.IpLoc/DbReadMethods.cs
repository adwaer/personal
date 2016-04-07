using System;
using System.IO.MemoryMappedFiles;
using MQ.Business;
using MQ.Cqrs.Factory;
using MQ.Domain;

namespace MQ.IpLoc
{
    internal class DbReadMethods
    {
        private const string FilePath = "geobase.dat";

        public static async void ReadUnsafe()
        {
            DateTime start = DateTime.Now;

            MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(FilePath);
            MemoryMappedViewStream stream = mmf.CreateViewStream();

            Helpers.WriteLog(ref start, "stream");

            IBinaryReader binaryReader = new UnmanagedReader(stream);

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
                indexes[i] = binaryReader.ReadInt32();
            }
            Helpers.WriteLog(ref start, "indexes");
        }
        
    }
}
