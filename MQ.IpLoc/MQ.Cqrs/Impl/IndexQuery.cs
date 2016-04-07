using System.Threading.Tasks;
using MQ.Business;

namespace MQ.Cqrs.Impl
{
    public class IndexQuery : IQuery<IBinaryReader, int, Task<float[]>>
    {
        public Task<float[]> Execute(IBinaryReader binaryReader, int recordCount)
        {
            return Task.Factory.StartNew(() =>
            {
                float[] indexes = new float[recordCount];
                for (int i = 0; i < recordCount; i++)
                {
                    indexes[i] = binaryReader.ReadInt32();
                }

                return indexes;
            });
        }
    }
}
