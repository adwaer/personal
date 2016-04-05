using System.IO;
using System.Threading.Tasks;
using MQ.Domain;

namespace MQ.Cqrs.Impl
{
    public class LocationQuery : IQuery<Stream, int, Task<Location[]>>
    {
        public Task<Location[]> Execute(Stream stream, int recordCount)
        {
            return Task.Factory.StartNew(() =>
            {
                Location[] locations = new Location[recordCount];
                using (var reader = new BinaryReader(stream))
                {
                    for (int i = 0; i < recordCount; i++)
                    {
                        locations[i] = Location.Get(reader);
                    }
                }

                return locations;
            });
        }
    }
}
