using System.IO;
using System.Threading.Tasks;
using MQ.Domain;

namespace MQ.Cqrs.Impl
{
    public class IpLocationQuery : IQuery<Stream, int, Task<IpLocation[]>>
    {
            public Task<IpLocation[]> Execute(Stream stream, int recordCount)
            {
                return Task.Factory.StartNew(() =>
                {
                    IpLocation[] ipLocations = new IpLocation[recordCount];
                    using (var reader = new BinaryReader(stream))
                    {
                        for (uint i = 0; i < recordCount; i++)
                        {
                            ipLocations[i] = IpLocation.Get(reader);
                        }
                    }

                    return ipLocations;
                });
            }
    }
}
