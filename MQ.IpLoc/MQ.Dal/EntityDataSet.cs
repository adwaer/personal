using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        public ICollection<Location> Locations { get; private set; }
        public float[] Indexes { get; private set; }

        public void Fetch()
        {
            using (var fr = new CustomFileReader())
            {
                if (!fr.Open(FilePath))
                {
                    Console.WriteLine("Не получилось открыть запрашиваемый файл");
                    return;
                }

                var headerBuffer = new byte[60];
                fr.Read(headerBuffer, 0, 60);
                var recordCount = BitConverter.ToInt32(headerBuffer, 44);

                ThreadPool.SetMaxThreads(Environment.ProcessorCount, Environment.ProcessorCount);

                // Async fetch for ip locations
                var ipLocWaiter = new ManualResetEventSlim();
                var ipLocations = new IpLocation[recordCount];
                var offset = 20 * recordCount;
                var buffer = new byte[offset];
                fr.Read(buffer, 0, offset);

                ThreadPool.QueueUserWorkItem(FetchIpLocations, new ThreadParams<IpLocation>
                {
                    Buffer = buffer,
                    RecordCount = recordCount,
                    Waiter = ipLocWaiter,
                    Result = ipLocations
                });

                // Async multithreaded fetch for ip locations
                int locWaiterThreadCount = Environment.ProcessorCount;
                var locWaiter = new Dictionary<int, LocationArgs>();
                for (int i = 0; i < locWaiterThreadCount; i++)
                {
                    locWaiter[i] = new LocationArgs(recordCount / locWaiterThreadCount);
                }

                offset = 96 * recordCount;
                for (int i = 0; i < locWaiterThreadCount; i++)
                {
                    buffer = new byte[offset / locWaiterThreadCount];
                    fr.Read(buffer, 0, offset / locWaiterThreadCount);
                    ThreadPool.QueueUserWorkItem(FetchLocations, new ThreadParams<Location>
                    {
                        Buffer = buffer,
                        RecordCount = recordCount / locWaiterThreadCount,
                        Waiter = locWaiter[i].Waiter,
                        Result = locWaiter[i].Data
                    });
                }


                // Async fetch for indexes
                offset = 4 * recordCount;
                buffer = new byte[offset];
                fr.Read(buffer, 0, offset);
                var indexWaiter = new ManualResetEventSlim();
                var indexes = new float[recordCount];

                ThreadPool.QueueUserWorkItem(FetchIndexes, new ThreadParams<float>
                {
                    Buffer = buffer,
                    RecordCount = recordCount,
                    Waiter = indexWaiter,
                    Result = indexes
                });

                // Fetch for header
                FetchHeader(headerBuffer, recordCount);

                ipLocWaiter.Wait();
                indexWaiter.Wait();

                var locations = new List<Location>(recordCount);
                for (int i = 0; i < locWaiterThreadCount; i++)
                {
                    var locationArgs = locWaiter[i];
                    locationArgs.Waiter.Wait();

                    locations.InsertRange(locWaiterThreadCount * i, locationArgs.Data);
                }


                IpLocations = ipLocations;
                Locations = locations;
                Indexes = indexes;
            }

        }

        #region private

        /// <summary>
        /// Fetch header
        /// </summary>
        /// <param name="buffer">byte array</param>
        /// <param name="recordCount">already fetched record count</param>
        private void FetchHeader(byte[] buffer, int recordCount)
        {
            int position = 0;

            var version = CustomBitConvert.ToInt32(buffer, position);
            position += 4;

            string name = CustomBitConvert.ToString(buffer, position, 32);
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

        /// <summary>
        /// Fetch ip locations
        /// </summary>
        /// <param name="data">Parameters for async thread starting (ThreadParams<IpLocation/>)</param>
        private static void FetchIpLocations(object data)
        {
            var pars = (ThreadParams<IpLocation>)data;
            var position = 0;
            var buffer = pars.Buffer;
            var recordCount = pars.RecordCount;

            var ipLocations = pars.Result;
            for (int i = 0; i < recordCount; i++)
            {
                var fromIp = CustomBitConvert.ToUInt64(buffer, position);
                position += 8;
                var toIp = CustomBitConvert.ToUInt64(buffer, position);
                position += 8;
                var index = CustomBitConvert.ToUInt32(buffer, position);
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
        /// <summary>
        /// Fetch locations
        /// </summary>
        /// <param name="data">Parameters for async thread starting (ThreadParams<Location/>)</param>
        private static void FetchLocations(object data)
        {
            var pars = (ThreadParams<Location>)data;
            var position = 0;
            var buffer = pars.Buffer;
            var recordCount = pars.RecordCount;

            var locations = pars.Result;
            for (int i = 0; i < recordCount; i++)
            {
                string country = CustomBitConvert.ToString(buffer, position, 8);
                position += 8;
                string region = CustomBitConvert.ToString(buffer, position, 12);
                position += 12;
                string postal = CustomBitConvert.ToString(buffer, position, 12);
                position += 12;
                string city = CustomBitConvert.ToString(buffer, position, 24);
                position += 24;
                string company = CustomBitConvert.ToString(buffer, position, 32);
                position += 32;
                var lat = CustomBitConvert.ToSingle(buffer, position);
                position += 4;
                var lon = CustomBitConvert.ToSingle(buffer, position);
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
        /// <summary>
        /// Fetch indexes
        /// </summary>
        /// <param name="data">Parameters for async thread starting (ThreadParams<float/>)</param>
        private static void FetchIndexes(object data)
        {
            var pars = (ThreadParams<float>)data;
            var position = 0;
            var buffer = pars.Buffer;
            var recordCount = pars.RecordCount;

            var indexes = pars.Result;
            for (int i = 0; i < recordCount; i++)
            {
                indexes[i] = CustomBitConvert.ToInt32(buffer, position);
                position += 4;
            }

            pars.Waiter.Set();
        }

        /// <summary>
        /// Arguments for multithread requests
        /// </summary>
        private class LocationArgs
        {
            public readonly ManualResetEventSlim Waiter;
            public readonly Location[] Data;

            public LocationArgs(int dataCount)
            {
                Waiter = new ManualResetEventSlim();
                Data = new Location[dataCount];
            }
        }

        #endregion
    }
}
