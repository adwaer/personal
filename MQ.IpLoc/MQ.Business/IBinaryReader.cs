using System;
using System.Text;

namespace MQ.Business
{
    public interface IBinaryReader : IDisposable
    {
        int ReadInt32();
        uint ReadUInt32();
        ulong ReadUInt64();
        float ReadFloat();
        DateTime ReadDateTime();
        string ReadString(int count, Encoding encoding = null);
    }
}
