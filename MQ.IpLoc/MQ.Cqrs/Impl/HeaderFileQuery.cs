using System.IO;
using MQ.Domain;

namespace MQ.Cqrs.Impl
{
    public class HeaderFileQuery : IQuery<Stream, Header>
    {
        public Header Execute(Stream stream)
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                return Header.Get(reader);
            }
        }
    }
}
