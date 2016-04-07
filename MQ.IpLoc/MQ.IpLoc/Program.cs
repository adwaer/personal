using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using MQ.Business;
using MQ.Cqrs.Factory;
using MQ.Domain;

namespace MQ.IpLoc
{
    internal class Program
    {
        private const string FilePath = "geobase.dat";
        private static void Main(string[] args)
        {
            DateTime start1 = DateTime.Now;
            DateTime start = DateTime.Now;

            //using (var stream = File.OpenRead("geobase.dat"))
            //{
            //    WriteLog(ref start, "read");
            //DbReadMethods.ReadByMethods(ref start); // 400 ms
            //DbReadMethods.ReadByQuery(ref start); // 500 ms
            //}
            WriteLog(ref start, "stage");
            //DbReadMethods.ReadUnsafe(ref start); // ??? ms

            MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(FilePath);
            MemoryMappedViewStream stream = mmf.CreateViewStream();

            WriteLog(ref start, "stream");

            IBinaryReader binaryReader = new UnmanagedReader(stream);

            WriteLog(ref start, "reader");

            // header
            var header = new HeaderFactory().Get(binaryReader);
            WriteLog(ref start, "header");

            // ip locations
            IpLocationFactory factory = new IpLocationFactory();
            var ipLocations = new IpLocation[header.RecordCount];
            for (uint i = 0; i < header.RecordCount; i++)
            {
                ipLocations[i] = factory.Get(binaryReader);
            }
            WriteLog(ref start, "ip locations");

            LocationFactory locationFactory = new LocationFactory();

            Location[] locations = new Location[header.RecordCount];
            for (int i = 0; i < header.RecordCount; i++)
            {
                locations[i] = locationFactory.Get(binaryReader);
            }
            WriteLog(ref start, "locations");

            float[] indexes = new float[header.RecordCount];
            for (int i = 0; i < header.RecordCount; i++)
            {
                indexes[i] = binaryReader.ReadInt32();
            }
            WriteLog(ref start, "indexes");

            WriteLog(ref start1, "spent");
            Console.ReadLine();
        }

        static void WriteLog(ref DateTime start, string action)
        {
            Console.WriteLine($"{action} time: {(DateTime.Now - start).TotalMilliseconds}");
            start = DateTime.Now;
        }
    }
}
