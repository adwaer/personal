using System.Threading.Tasks;
using MQ.Business;
using MQ.Domain;

namespace MQ.Cqrs.Factory
{
    public class HeaderFactory : IEntityFactory<Task<Header>>
    {
        public async Task<Header> Get(IBinaryReader reader)
        {
            return new Header
            {
                Version = reader.ReadInt32(),
                Name = await reader.ReadString(32),
                MakeTime = await reader.ReadDateTime(),
                RecordCount = reader.ReadInt32(),
                RangeOffset = await reader.ReadUInt32(),
                CityOffset = await reader.ReadUInt32(),
                LocationOffset = await reader.ReadUInt32()
            };
        }
    }
}