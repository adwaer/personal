using System;
using System.IO;
using System.Text;
using MQ.Domain;

namespace MQ.Business
{
    public class ManagedReader : IBinaryReader
    {
        private readonly BinaryReader _binaryReader;
        private readonly Stream _stream;

        public ManagedReader(string fileName)
        {
            _stream = File.OpenRead(fileName);
            _binaryReader = new BinaryReader(_stream);
        }

        public int ReadInt32()
        {
            return _binaryReader.ReadInt32();
        }

        public uint ReadUInt32()
        {
            return _binaryReader.ReadUInt32();
        }

        public ulong ReadUInt64()
        {
            return _binaryReader.ReadUInt64();
        }

        public float ReadFloat()
        {
            return _binaryReader.ReadSingle();
        }

        public DateTime ReadDateTime()
        {
            return EnvironmentConstant
                .UnixDateTime
                .AddSeconds(_binaryReader.ReadInt64())
                .ToLocalTime();
        }

        public string ReadString(int count, Encoding encoding = null)
        {
            return (encoding ?? Encoding.Default)
                .GetString(_binaryReader.ReadBytes(count))
                .TrimEnd(EnvironmentConstant.EmptySpace);
        }

        public void Dispose()
        {
            _stream.Dispose();
            _binaryReader.Dispose();
        }
    }
}
