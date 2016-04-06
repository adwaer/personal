using System;
using System.Runtime.InteropServices;
using System.Text;
using MQ.Domain;

namespace MQ.IpLoc
{
    public class UnsafeFileReader : IDisposable
    {
        const uint GenericRead = 0x80000000;
        const uint OpenExisting = 3;
        IntPtr _handle;

        [DllImport("kernel32", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Unicode)]
        static extern IntPtr CreateFile
        (
            string FileName,          // file name
            uint DesiredAccess,       // access mode
            uint ShareMode,           // share mode
            uint SecurityAttributes,  // Security Attributes
            uint CreationDisposition, // how to create
            uint FlagsAndAttributes,  // file attributes
            int hTemplateFile         // handle to template file
        );

        [DllImport("kernel32", SetLastError = true)]
        static extern unsafe bool ReadFile
        (
            IntPtr hFile,      // handle to file
            void* pBuffer,            // data buffer
            int NumberOfBytesToRead,  // number of bytes to read
            int* pNumberOfBytesRead,  // number of bytes read
            int Overlapped            // overlapped buffer
        );

        [DllImport("kernel32", SetLastError = true)]
        static extern bool CloseHandle
        (
            IntPtr hObject // handle to object
        );

        public bool Open(string fileName)
        {
            // open the existing file for reading       
            _handle = CreateFile
            (
                fileName,
                GenericRead,
                0,
                0,
                OpenExisting,
                0,
                0
            );

            return _handle != IntPtr.Zero;
        }

        public unsafe int Read(byte[] buffer, int index, int count)
        {
            int n = 0;
            fixed (byte* p = buffer)
            {
                if (!ReadFile(_handle, p + index, count, &n, 0))
                {
                    return 0;
                }
            }
            return n;
        }

        private const int Four = 4;
        private const int Eight = 8;

        public int ReadInt32()
        {
            var buffer = new byte[Four];
            Read(buffer, 0, Four);

            return BitConverter.ToInt32(buffer, 0);
        }

        public string ReadString(int count, Encoding encoding = null)
        {
            var buffer = new byte[count];
            int bytesRead = Read(buffer, 0, buffer.Length);

            return (encoding ?? Encoding.Default)
                .GetString(buffer, 0, bytesRead)
                .TrimEnd(EnvironmentConstant.EmptySpace);
        }

        public bool Close()
        {
            return CloseHandle(_handle);
        }

        public void Dispose()
        {
            Close();
        }

        public uint ReadUInt32()
        {
            var buffer = new byte[Four];
            Read(buffer, 0, Four);

            return BitConverter.ToUInt32(buffer, 0);
        }

        public ulong ReadUInt64()
        {
            var buffer = new byte[Eight];
            Read(buffer, 0, Eight);

            return BitConverter.ToUInt64(buffer, 0);
        }

        public float ReadSingle()
        {
            var buffer = new byte[Four];
            Read(buffer, 0, Four);

            return BitConverter.ToSingle(buffer, 0);
        }

        public long ReadInt64()
        {
            var buffer = new byte[Eight];
            Read(buffer, 0, Eight);

            return BitConverter.ToInt64(buffer, 0);
        }
    }
}
