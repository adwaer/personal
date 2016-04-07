using System;
using System.Text;
using System.Threading.Tasks;

namespace MQ.Business
{
    public interface IBinaryReader
    {
        Task<int> ReadInt32();
        Task<uint> ReadUInt32();
        Task<ulong> ReadUInt64();
        Task<float> ReadFloat();
        Task<DateTime> ReadDateTime();
        Task<string> ReadString(int count, Encoding encoding = null);
    }
}
