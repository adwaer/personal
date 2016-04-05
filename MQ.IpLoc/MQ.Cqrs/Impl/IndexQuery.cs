using System.IO;
using System.Threading.Tasks;

namespace MQ.Cqrs.Impl
{
    public class IndexQuery : IQuery<Stream, int, Task<float[]>>
    {
        public Task<float[]> Execute(Stream stream, int recordCount)
        {
            return Task.Factory.StartNew(() =>
            {
                float[] indexes = new float[recordCount];
                using (var reader = new BinaryReader(stream))
                {
                    for (int i = 0; i < recordCount; i++)
                    {
                        indexes[i] = reader.ReadInt32();
                    }
                }

                return indexes;
            });
        }
    }
}
