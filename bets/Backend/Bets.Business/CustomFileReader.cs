using System;
using System.Runtime.InteropServices;

namespace Bets.Business
{
    /// <summary>
    /// Binary unsafe file reader
    /// </summary>
    public class CustomFileReader : IDisposable
    {
        const uint GENERIC_READ = 0x80000000;
        const uint OPEN_EXISTING = 3;
        IntPtr handle;

        [DllImport("kernel32", SetLastError = true)]
        static extern unsafe IntPtr CreateFile(
              string FileName,                    // имя файла
              uint DesiredAccess,                 // режим доступа
              uint ShareMode,                     // режим общего использования
              uint SecurityAttributes,            // атрибуты безопасности
              uint CreationDisposition,           // как создавать
              uint FlagsAndAttributes,            // атрибуты файла
              int hTemplateFile                   // handle для шаблона файла
              );
        [DllImport("kernel32", SetLastError = true)]
        static extern unsafe bool ReadFile(
              IntPtr hFile,                       // handle файла
              void* pBuffer,                      // буфер данных
              int NumberOfBytesToRead,            // количество байт для чтения
              int* pNumberOfBytesRead,            // количество прочитанных байт
              int Overlapped                      // здесь должен быть указатель на 
                                                  // структуру overlapped, но в данном
                                                  // примере она не используется, так
                                                  // что тут просто int.
              );
        [DllImport("kernel32", SetLastError = true)]
        static extern unsafe bool CloseHandle(
              IntPtr hObject   // handle объекта
              );

        public bool Open(string fileName)
        {
            // open the existing file for reading          
            handle = CreateFile(fileName,
                                GENERIC_READ,
                                0,
                                0,
                                OPEN_EXISTING,
                                0,
                                0);

            return handle != IntPtr.Zero;
        }

        public unsafe void Read(byte[] buffer, int index, int count)
        {
            int n = 0;
            fixed (byte* p = buffer)
            {
                if (!ReadFile(handle, p + index, count, &n, 0))
                    throw new AccessViolationException();
            }
        }

        public bool Close()
        {
            // закрыть хендл файла
            return CloseHandle(handle);
        }

        public void Dispose()
        {
            Close();
        }
    }
}
