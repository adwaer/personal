using MQ.Domain;

namespace MQ.Business.Factory
{
    public class HeaderEntityFactory : IEntityFactory<Header>
    {
        public Header Get(IBinaryReader reader)
        {
            return new Header
            {
                Version = reader.ReadInt32(),
                Name = reader.ReadString(32),
                MakeTime = reader.ReadDateTime(),
                RecordCount = reader.ReadInt32(),
                RangeOffset = reader.ReadUInt32(),
                CityOffset = reader.ReadUInt32(),
                LocationOffset = reader.ReadUInt32()
            };
        }
    }
}