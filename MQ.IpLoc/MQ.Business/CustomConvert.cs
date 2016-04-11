using System;

namespace MQ.Business
{
    public class CustomConvert
    {
        public unsafe static string ToString(byte[] bytes, int offset, int length)
        {
            fixed (byte* p = bytes)
            {
                return new string((sbyte*)p, offset, length);
            }
        }

        public static unsafe Func<byte[], int, int> ToInt32;

        static CustomConvert()
        {
            if (BitConverter.IsLittleEndian)
            {
                ToInt32 = ToInt32LittleEndian;
            }
            else
            {
                ToInt32 = ToInt32NotLittleEndian;
            }
        }

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

        public unsafe static float ToSingle(byte[] bytes, int offset)
        {
            return *(float*)ToInt32(bytes, offset);
        }
    }
}
