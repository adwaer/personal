using System.Threading.Tasks;
using MQ.Business;
using MQ.Domain;

namespace MQ.Cqrs.Factory
{
    public class IpLocationFactory : IEntityFactory<Task<IpLocation>>
    {
        public async Task<IpLocation> Get(IBinaryReader reader)
        {
            return new IpLocation
            {
                FromIp = await reader.ReadUInt64(),
                ToIp = await reader.ReadUInt64(),
                Index = await reader.ReadUInt32()
            };
        }
    }
    
}