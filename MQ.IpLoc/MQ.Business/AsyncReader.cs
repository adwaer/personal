using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MQ.Domain;

namespace MQ.Business
{
    public class AsyncReader : IBinaryReader
    {
        private readonly Stream _stream;

        public AsyncReader(Stream stream)
        {
            _stream = stream;
        }

        private const int Four = 4;
        private const int Eight = 8;

        public async Task<int> ReadInt32()
        {
            var buffer = new byte[Four];
            await _stream.ReadAsync(buffer, 0, Four);

            return BitConverter.ToInt32(buffer, 0);
        }

        public async Task<long> ReadInt64()
        {
            var buffer = new byte[Eight];
            await _stream.ReadAsync(buffer, 0, Eight);

            return BitConverter.ToInt64(buffer, 0);
        }

        public async Task<uint> ReadUInt32()
        {
            var buffer = new byte[Four];
            await _stream.ReadAsync(buffer, 0, Four);

            return BitConverter.ToUInt32(buffer, 0);
        }

        public async Task<ulong> ReadUInt64()
        {
            var buffer = new byte[Eight];
            await _stream.ReadAsync(buffer, 0, Eight);

            return BitConverter.ToUInt64(buffer, 0);
        }

        public async Task<float> ReadFloat()
        {
            var buffer = new byte[Four];
            await _stream.ReadAsync(buffer, 0, Four);

            return BitConverter.ToSingle(buffer, 0);
        }

        public async Task<DateTime> ReadDateTime()
        {
            return EnvironmentConstant
                .UnixDateTime
                .AddSeconds(await ReadInt64())
                .ToLocalTime();
        }

        public async Task<string> ReadString(int count, Encoding encoding = null)
        {
            var buffer = new byte[count];
            int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);

            return (encoding ?? Encoding.Default)
                .GetString(buffer, 0, bytesRead)
                .TrimEnd(EnvironmentConstant.EmptySpace);
        }
    }
}