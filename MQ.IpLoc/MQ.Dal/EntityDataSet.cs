using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using MQ.Business;
using MQ.Domain;

namespace MQ.Dal
{
    public class EntityDataSet
    {
        private const string FilePath = "geobase.dat";
        public Header Header { get; private set; }
        public IpLocation[] IpLocations { get; private set; }
        public Location[] Locations { get; private set; }
        public float[] Indexes { get; private set; }

        public void Fetch()
        {
            using (FileReader fr = new FileReader())
            {
                if (!fr.Open(FilePath))
                {
                    Console.WriteLine("Не получилось открыть запрашиваемый файл");
                    return;
                }

                var headerBuffer = new byte[60];
                fr.Read(headerBuffer, 0, 60);

                var recordCount = BitConverter.ToInt32(headerBuffer, 44);

                var ipLocWaiter = new ManualResetEventSlim();
                var locWaiter = new ManualResetEventSlim();
                var indexWaiter = new ManualResetEventSlim();

                var ipLocations = new IpLocation[recordCount];
                var locations = new Location[recordCount];
                var indexes = new float[recordCount];

                var length = 20 * recordCount;
                var buffer = new byte[length];
                fr.Read(buffer, 0, length);
                ThreadPool.QueueUserWorkItem(FetchIpLocations, new ThreadParams<IpLocation>
                {
                    Buffer = buffer,
                    RecordCount = recordCount,
                    Waiter = ipLocWaiter,
                    Result = ipLocations
                });

                length = 96 * recordCount;
                buffer = new byte[length];
                fr.Read(buffer, 0, length);
                ThreadPool.QueueUserWorkItem(FetchLocations, new ThreadParams<Location>
                {
                    Buffer = buffer,
                    RecordCount = recordCount,
                    Waiter = locWaiter,
                    Result = locations
                });

                length = 4 * recordCount;
                buffer = new byte[length];
                fr.Read(buffer, 0, length);
                ThreadPool.QueueUserWorkItem(FetchIndexes, new ThreadParams<float>
                {
                    Buffer = buffer,
                    RecordCount = recordCount,
                    Waiter = indexWaiter,
                    Result = indexes
                });

                FetchHeader(headerBuffer, recordCount);

                ipLocWaiter.Wait();
                locWaiter.Wait();
                indexWaiter.Wait();

                IpLocations = ipLocations;
                Locations = locations;
                Indexes = indexes;
            }

        }

        private void FetchHeader(byte[] buffer, int recordCount)
        {
            int position = 0;

            var version = CustomConvert.ToInt32(buffer, position);
            position += 4;

            string name = CustomConvert.ToString(buffer, position, 32);
            position += 32;

            var makeTime = EnvironmentConstant.UnixDateTime.AddSeconds(BitConverter.ToInt64(buffer, position));
            position += 8;

            //var recordCount = BitConverter.ToInt32(buffer, position);
            position += 4;

            var rangeOffset = BitConverter.ToUInt32(buffer, position);
            position += 4;

            var cityOffset = BitConverter.ToUInt32(buffer, position);
            position += 4;

            var locationOffset = BitConverter.ToUInt32(buffer, position);

            Header = new Header
            {
                RecordCount = recordCount,
                RangeOffset = rangeOffset,
                LocationOffset = locationOffset,
                CityOffset = cityOffset,
                MakeTime = makeTime,
                Name = name,
                Version = version
            };
        }

        private static void FetchIpLocations(object data)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var pars = (ThreadParams<IpLocation>)data;
            var position = 0;
            var buffer = pars.Buffer;
            var recordCount = pars.RecordCount;

            var ipLocations = pars.Result;
            for (int i = 0; i < recordCount; i++)
            {
                var fromIp = BitConverter.ToUInt64(buffer, position);
                position += 8;
                var toIp = BitConverter.ToUInt64(buffer, position);
                position += 8;
                var index = BitConverter.ToUInt32(buffer, position);
                position += 4;

                ipLocations[i] = new IpLocation
                {
                    FromIp = fromIp,
                    ToIp = toIp,
                    Index = index
                };
            }

            stopwatch.Stop();
            Console.WriteLine($"ip locations time: {stopwatch.ElapsedMilliseconds}ms");


            pars.Waiter.Set();
        }

        private static void FetchLocations(object data)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var pars = (ThreadParams<Location>)data;
            var position = 0;
            var buffer = pars.Buffer;
            var recordCount = pars.RecordCount;

            var locations = pars.Result;
            for (int i = 0; i < recordCount; i++)
            {
                string country = CustomConvert.ToString(buffer, position, 8);
                position += 8;
                string region = CustomConvert.ToString(buffer, position, 12);
                position += 12;
                string postal = CustomConvert.ToString(buffer, position, 12);
                position += 12;
                string city = CustomConvert.ToString(buffer, position, 24);
                position += 24;
                string company = CustomConvert.ToString(buffer, position, 32);
                position += 32;
                var lat = CustomConvert.ToSingle(buffer, position);
                position += 4;
                var lon = CustomConvert.ToSingle(buffer, position);
                position += 4;

                locations[i] = new Location
                {
                    Country = country,
                    Region = region,
                    Postal = postal,
                    City = city,
                    Company = company,
                    Lat = lat,
                    Lon = lon
                };
            }

            stopwatch.Stop();
            Console.WriteLine($"locations time: {stopwatch.ElapsedMilliseconds}ms");

            pars.Waiter.Set();
        }

        private static void FetchIndexes(object data)
        {
            var pars = (ThreadParams<float>)data;
            var position = 0;
            var buffer = pars.Buffer;
            var recordCount = pars.RecordCount;

            var indexes = pars.Result;
            for (int i = 0; i < recordCount; i++)
            {
                indexes[i] = BitConverter.ToInt32(buffer, position);
                position += 4;
            }

            pars.Waiter.Set();
        }
        
    }
}
