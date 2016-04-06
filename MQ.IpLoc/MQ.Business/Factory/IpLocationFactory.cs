using MQ.Domain;

namespace MQ.Business.Factory
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