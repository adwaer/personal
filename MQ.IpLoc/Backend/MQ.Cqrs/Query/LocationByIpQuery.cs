using System;
using System.Linq;
using System.Threading.Tasks;
using MQ.Business;
using MQ.Dal;
using MQ.Domain;

namespace MQ.Cqrs.Query
{
    /// <summary>
    /// Get Location by ip
    /// </summary>
    public class LocationByIpQuery : IQuery<string, Task<Location>>
    {
        private readonly EntityDataSet _entityDataSet;

        public LocationByIpQuery(EntityDataSet entityDataSet)
        {
            _entityDataSet = entityDataSet;
        }

        public async Task<Location> Execute(string ip)
        {
            ulong ipLong = CustomBitConvert.Ip2Long(ip);
            var ipLocation = _entityDataSet
                .IpLocations
                .FirstOrDefault(iploc => iploc.FromIp <= ipLong && iploc.ToIp >= ipLong);

            if (ipLocation == null)
            {
                return await Task.FromResult<Location>(null);
            }

            return ipLocation.Location ??
                (ipLocation.Location = await GetLocationByIndex(ipLocation.Index));
        }

        public async Task<Location> GetLocationByIndex(uint index)
        {
            return await Task.Factory.StartNew(() =>
            {
                int locationIndex = Array
                    .FindIndex(EntityDataSet
                        .Instance
                        .Indexes,
                        i => i == index);

                if (locationIndex < 0)
                {
                    throw new IndexOutOfRangeException("ip location index is not exists");
                }

                return EntityDataSet
                    .Instance
                    .Locations
                    .ElementAt(locationIndex);
            });
        }
    }
}
