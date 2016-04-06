using System;
using System.Text;
using MQ.Domain;
using MQ.IpLoc;

namespace MQ.Business
{
    public class UnmanagedReader : IBinaryReader
    {
        private readonly UnsafeFileReader _reader;

        public UnmanagedReader(UnsafeFileReader reader)
        {
            _reader = reader;
        }

        public void Dispose()
        {
            _reader.Dispose();
        }

        public int ReadInt32()
        {
            return _reader.ReadInt32();
        }

        public uint ReadUInt32()
        {
            return _reader.ReadUInt32();
        }

        public ulong ReadUInt64()
        {
            return _reader.ReadUInt64();
        }

        public float ReadFloat()
        {
            return _reader.ReadSingle();
        }

        public DateTime ReadDateTime()
        {
            return EnvironmentConstant
                .UnixDateTime
                .AddSeconds(_reader.ReadInt64())
                .ToLocalTime();
        }

        public string ReadString(int count, Encoding encoding = null)
        {
            return _reader.ReadString(count, encoding);
        }
    }
}