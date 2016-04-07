using System.Threading.Tasks;
using MQ.Business;
using MQ.Cqrs.Factory;
using MQ.Domain;

namespace MQ.Cqrs.Impl
{
    public class LocationQuery : IQuery<IBinaryReader, int, Task<Location[]>>
    {
        public Task<Location[]> Execute(IBinaryReader binaryReader, int recordCount)
        {
            return Task.Factory.StartNew(() =>
            {
                LocationFactory factory = new LocationFactory();

                Location[] locations = new Location[recordCount];
                for (int i = 0; i < recordCount; i++)
                {
                    locations[i] = factory.Get(binaryReader);
                }

                return locations;
            });
        }
    }
}
