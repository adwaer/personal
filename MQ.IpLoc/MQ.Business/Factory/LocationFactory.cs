using MQ.Domain;

namespace MQ.Business.Factory
{
    public class LocationFactory : IEntityFactory<Location>
    {
        public Location Get(IBinaryReader reader)
        {
            return new Location
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
    }
}