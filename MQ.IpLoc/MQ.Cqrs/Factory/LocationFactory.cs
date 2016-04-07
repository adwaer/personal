using System.Threading.Tasks;
using MQ.Business;
using MQ.Domain;

namespace MQ.Cqrs.Factory
{
    public class LocationFactory : IEntityFactory<Task<Location>>
    {
        public async Task<Location> Get(IBinaryReader reader)
        {
            return new Location
            {
                Country = await reader.ReadString(8),
                Region = await reader.ReadString(12),
                Postal = await reader.ReadString(12),
                City = await reader.ReadString(24),
                Company = await reader.ReadString(32),
                Lat = await reader.ReadFloat(),
                Lon = await reader.ReadFloat()
            };
        }
    }
}