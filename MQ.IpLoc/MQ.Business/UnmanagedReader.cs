using System;
using System.IO;
using System.Text;
using MQ.Domain;

namespace MQ.Business
{
    public class UnmanagedReader : IBinaryReader
    {
        private readonly Stream _stream;

        public UnmanagedReader(Stream stream)
        {
            _stream = stream;
        }

        private const int Four = 4;
        private const int Eight = 8;

        public int ReadInt32()
        {
            var buffer = new byte[Four];
            _stream.Read(buffer, 0, Four);

            return BitConverter.ToInt32(buffer, 0);
        }

        public long ReadInt64()
        {
            var buffer = new byte[Eight];
            _stream.Read(buffer, 0, Eight);

            return BitConverter.ToInt64(buffer, 0);
        }

        public uint ReadUInt32()
        {
            var buffer = new byte[Four];
            _stream.Read(buffer, 0, Four);

            return BitConverter.ToUInt32(buffer, 0);
        }

        public ulong ReadUInt64()
        {
            var buffer = new byte[Eight];
            _stream.Read(buffer, 0, Eight);

            return BitConverter.ToUInt64(buffer, 0);
        }

        public float ReadFloat()
        {
            var buffer = new byte[Four];
            _stream.Read(buffer, 0, Four);

            return BitConverter.ToSingle(buffer, 0);
        }

        public DateTime ReadDateTime()
        {
            return EnvironmentConstant
                .UnixDateTime
                .AddSeconds(ReadInt64())
                .ToLocalTime();
        }

        public string ReadString(int count, Encoding encoding = null)
        {
            var buffer = new byte[count];
            int bytesRead = _stream.Read(buffer, 0, buffer.Length);

            return (encoding ?? Encoding.Default)
                .GetString(buffer, 0, bytesRead)
                .TrimEnd(EnvironmentConstant.EmptySpace);
        }
    }
}