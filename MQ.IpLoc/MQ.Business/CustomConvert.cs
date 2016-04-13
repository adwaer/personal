using System;
using System.Security;

namespace MQ.Business
{
    public class CustomConvert
    {
        public static Func<byte[], int, int> ToInt32;
        public static Func<byte[], int, long> ToInt64;

        static CustomConvert()
        {
            if (BitConverter.IsLittleEndian)
            {
                ToInt32 = ToInt32LittleEndian;
                ToInt64 = ToInt64LittleEndian;
            }
            else
            {
                ToInt32 = ToInt32NotLittleEndian;
                ToInt64 = ToInt64NotLittleEndian;
            }
        }

        public unsafe static string ToString(byte[] bytes, int offset, int length)
        {
            fixed (byte* p = bytes)
            {
                return new string((sbyte*)p, offset, length);
            }
        }

        [SecuritySafeCritical]
        public static unsafe float ToSingle(byte[] bytes, int offset)
        {
            fixed (byte* numPtr = &bytes[offset])
            {
                if (offset % 4 == 0)
                    return *(float*)numPtr;

                return *(float*)(*numPtr | numPtr[1] << 8 | numPtr[2] << 16 | numPtr[3] << 24);
            }
        }

        public static ulong ToUInt64(byte[] bytes, int offset)
        {
            return (ulong)ToInt64(bytes, offset);
        }

        public static uint ToUInt32(byte[] bytes, int offset)
        {
            return (uint)BitConverter.ToInt32(bytes, offset);
        }

        #region private
        private unsafe static int ToInt32NotLittleEndian(byte[] bytes, int offset)
        {
            fixed (byte* numPtr = &bytes[offset])
            {
                if (offset % 4 == 0)
                    return *(int*)numPtr;

                return *numPtr << 24 | numPtr[1] << 16 | numPtr[2] << 8 | numPtr[3];
            }
        }

        private unsafe static int ToInt32LittleEndian(byte[] bytes, int offset)
        {
            fixed (byte* numPtr = &bytes[offset])
            {
                if (offset % 4 == 0)
                    return *(int*)numPtr;

                return *numPtr | numPtr[1] << 8 | numPtr[2] << 16 | numPtr[3] << 24;
            }
        }

        private static unsafe long ToInt64NotLittleEndian(byte[] value, int startIndex)
        {
            fixed (byte* numPtr = &value[startIndex])
            {
                if (startIndex % 8 == 0)
                    return *(long*)numPtr;

                int num = *numPtr << 24 | numPtr[1] << 16 | numPtr[2] << 8 | numPtr[3];
                return (uint)(numPtr[4] << 24 | numPtr[5] << 16 | numPtr[6] << 8) | numPtr[7] | (long)num << 32;
            }
        }

        private static unsafe long ToInt64LittleEndian(byte[] value, int startIndex)
        {
            fixed (byte* numPtr = &value[startIndex])
            {
                if (startIndex % 8 == 0)
                    return *(long*)numPtr;

                return (uint)(*numPtr | numPtr[1] << 8 | numPtr[2] << 16 | numPtr[3] << 24) | (long)(numPtr[4] | numPtr[5] << 8 | numPtr[6] << 16 | numPtr[7] << 24) << 32;
            }
        }
        #endregion
    }
}
