using MQ.Business;
using MQ.Domain;

namespace MQ.Cqrs.Factory
{
    public class IpLocationFactory : IEntityFactory<IpLocation>
    {
        public IpLocation Get(IBinaryReader reader)
        {
            return new IpLocation
            {
                FromIp = reader.ReadUInt64(),
                ToIp = reader.ReadUInt64(),
                Index = reader.ReadUInt32()
            };
        }
    }
}