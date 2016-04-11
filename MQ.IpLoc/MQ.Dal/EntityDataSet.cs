using System;
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
            byte[] buffer;
            using (FileReader fr = new FileReader())
            {
                if (!fr.Open(FilePath))
                {
                    Console.WriteLine("Не получилось открыть запрашиваемый файл");
                    return;
                }

                var fileInfo = new FileInfo(FilePath);
                buffer = new byte[fileInfo.Length];
                fr.Read(buffer, 0, (int)fileInfo.Length);
            }
            
            int position = 0;

            var version = BitConverter.ToInt32(buffer, position);
            position += 4;

            string name = Encoding.Default.GetString(buffer, position, 32);
            position += 32;

            var makeTime = EnvironmentConstant.UnixDateTime.AddSeconds(BitConverter.ToInt64(buffer, position));
            position += 8;

            var recordCount = BitConverter.ToInt32(buffer, position);
            position += 4;

            var rangeOffset = BitConverter.ToUInt32(buffer, position);
            position += 4;

            var cityOffset = BitConverter.ToUInt32(buffer, position);
            position += 4;

            var locationOffset = BitConverter.ToUInt32(buffer, position);
            position += 4;

            var ipLocWaiter = new ManualResetEventSlim();
            var locWaiter = new ManualResetEventSlim();
            var indexWaiter = new ManualResetEventSlim();

            var ipLocations = new IpLocation[recordCount];
            var locations = new Location[recordCount];
            var indexes = new float[recordCount];

            ThreadPool.QueueUserWorkItem(FetchIpLocations, new ThreadParams<IpLocation>
            {
                Buffer = buffer,
                Position = position,
                RecordCount = recordCount,
                Waiter = ipLocWaiter,
                Result = ipLocations
            });
            position += 20 * recordCount;
            ThreadPool.QueueUserWorkItem(FetchLocations, new ThreadParams<Location>
            {
                Buffer = buffer,
                Position = position,
                RecordCount = recordCount,
                Waiter = locWaiter,
                Result = locations
            });
            position += 96 * recordCount;
            ThreadPool.QueueUserWorkItem(FetchIndexes, new ThreadParams<float>
            {
                Buffer = buffer,
                Position = position,
                RecordCount = recordCount,
                Waiter = indexWaiter,
                Result = indexes
            });

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

            ipLocWaiter.Wait();
            locWaiter.Wait();
            indexWaiter.Wait();

            IpLocations = ipLocations;
            Locations = locations;
            Indexes = indexes;
        }

        private static void FetchIpLocations(object data)
        {
            var pars = (ThreadParams<IpLocation>)data;
            var position = pars.Position;
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

            pars.Waiter.Set();
        }

        private static void FetchLocations(object data)
        {
            var pars = (ThreadParams<Location>)data;
            var position = pars.Position;
            var buffer = pars.Buffer;
            var recordCount = pars.RecordCount;
            Encoding ansiEncoding = Encoding.Default;

            var locations = pars.Result;
            for (int i = 0; i < recordCount; i++)
            {
                string country = ansiEncoding.GetString(buffer, position, 8);
                position += 8;
                string region = ansiEncoding.GetString(buffer, position, 12);
                position += 12;
                string postal = ansiEncoding.GetString(buffer, position, 12);
                position += 12;
                string city = ansiEncoding.GetString(buffer, position, 24);
                position += 24;
                string company = ansiEncoding.GetString(buffer, position, 32);
                position += 32;
                var lat = BitConverter.ToInt32(buffer, position);
                position += 4;
                var lon = BitConverter.ToInt32(buffer, position);
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

            pars.Waiter.Set();
        }

        private static void FetchIndexes(object data)
        {
            var pars = (ThreadParams<float>)data;
            var position = pars.Position;
            var buffer = pars.Buffer;
            var recordCount = pars.RecordCount;

            var indexes = pars.Result;
            for (int i = 0; i < recordCount; i++)
            {
                indexes[i] = BitConverter.ToSingle(buffer, position);
                position += 4;
            }

            pars.Waiter.Set();
        }
    }
}
