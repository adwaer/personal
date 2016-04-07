using System;
using System.IO;
using System.Text;
using MQ.Domain;

namespace MQ.Business
{
    public class ByteBinaryReader
    {
        private readonly byte[] _fileBuffer;
        public int Position;

        private const int Four = 4;
        private const int Eight = 8;

        public ByteBinaryReader(string fileName)
        {
            _fileBuffer = ReadFileBuffer(fileName);
        }

        private static byte[] ReadFileBuffer(string fileName)
        {
            WinFileIO reader = null;

            try
            {
                unsafe
                {
                    byte[] buffer = new byte[(new FileInfo(fileName)).Length];

                    //byte[] buffer = new byte[4];

                    reader = new WinFileIO(buffer);
                    reader.OpenForReading(fileName);
                    reader.Read(buffer.Length);

                    return buffer;
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
        }

        public DateTime ReadDateTime()
        {
            return EnvironmentConstant
                .UnixDateTime
                .AddSeconds(ReadInt64())
                .ToLocalTime();
        }

        private long ReadInt64()
        {
            byte[] buffer = new byte[Eight];
            Array.Copy(_fileBuffer, Position, buffer, 0, Eight);
            Position += Eight;

            return BitConverter.ToInt64(buffer, 0);
        }
        private long ReadInt64(int fromPosition)
        {
            byte[] buffer = new byte[Eight];
            Array.Copy(_fileBuffer, fromPosition, buffer, 0, Eight);

            return BitConverter.ToInt64(buffer, 0);
        }

        public float ReadFloat()
        {
            byte[] buffer = new byte[Four];
            Array.Copy(_fileBuffer, Position, buffer, 0, Four);
            Position += Four;

            return BitConverter.ToSingle(buffer, 0);
        }
        public float ReadFloat(int fromPosition)
        {
            byte[] buffer = new byte[Four];
            Array.Copy(_fileBuffer, fromPosition, buffer, 0, Four);

            return BitConverter.ToSingle(buffer, 0);
        }

        public int ReadInt32()
        {
            byte[] buffer = new byte[Four];
            Array.Copy(_fileBuffer, Position, buffer, 0, Four);
            Position += Four;

            return BitConverter.ToInt32(buffer, 0);
        }

        public int ReadInt32(int fromPosition)
        {
            byte[] buffer = new byte[Four];
            Array.Copy(_fileBuffer, fromPosition, buffer, 0, Four);

            return BitConverter.ToInt32(buffer, 0);
        }

        public string ReadString(int count)
        {
            byte[] buffer = new byte[count];
            Array.Copy(_fileBuffer, Position, buffer, 0, count);
            Position += count;

            //return BitConverter.ToString(buffer, 0, count);
            return Encoding.Default
                .GetString(buffer, 0, count)
                .TrimEnd(EnvironmentConstant.EmptySpace);
        }

        public string ReadString(int count, int fromPosition)
        {
            byte[] buffer = new byte[count];
            Array.Copy(_fileBuffer, fromPosition, buffer, 0, count);

            //return BitConverter.ToString(buffer, 0, count);
            return Encoding.Default
                .GetString(buffer, 0, count)
                .TrimEnd(EnvironmentConstant.EmptySpace);
        }

        public uint ReadUInt32()
        {
            byte[] buffer = new byte[Four];
            Array.Copy(_fileBuffer, Position, buffer, 0, Four);
            Position += Four;

            return BitConverter.ToUInt32(buffer, 0);
        }

        public uint ReadUInt32(int fromPosition)
        {
            byte[] buffer = new byte[Four];
            Array.Copy(_fileBuffer, fromPosition, buffer, 0, Four);

            return BitConverter.ToUInt32(buffer, 0);
        }

        public ulong ReadUInt64()
        {
            byte[] buffer = new byte[Eight];
            Array.Copy(_fileBuffer, Position, buffer, 0, Eight);
            Position += Eight;

            return BitConverter.ToUInt64(buffer, 0);
        }
        public ulong ReadUInt64(int fromPosition)
        {
            byte[] buffer = new byte[Eight];
            Array.Copy(_fileBuffer, fromPosition, buffer, 0, Eight);

            return BitConverter.ToUInt64(buffer, 0);
        }
    }
}