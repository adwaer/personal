using System.Threading.Tasks;
using MQ.Business;
using MQ.Cqrs.Factory;
using MQ.Domain;

namespace MQ.Cqrs.Impl
{
    public class IpLocationQuery : IQuery<IBinaryReader, int, Task<IpLocation[]>>
    {
        public Task<IpLocation[]> Execute(IBinaryReader binaryReader, int recordCount)
        {
            return Task.Factory.StartNew(() =>
            {
                IpLocationFactory factory = new IpLocationFactory();

                IpLocation[] ipLocations = new IpLocation[recordCount];
                for (uint i = 0; i < recordCount; i++)
                {
                    ipLocations[i] = factory.Get(binaryReader);
                }

                return ipLocations;
            });
        }
    }
}
