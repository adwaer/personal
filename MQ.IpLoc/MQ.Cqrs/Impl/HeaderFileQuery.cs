using System.IO;
using MQ.Business;
using MQ.Cqrs.Factory;
using MQ.Domain;

namespace MQ.Cqrs.Impl
{
    public class HeaderFileQuery : IQuery<IBinaryReader, Header>
    {
        public Header Execute(IBinaryReader binaryReader)
        {
            var factory = new HeaderFactory();
            return factory.Get(binaryReader);
        }
    }
}
