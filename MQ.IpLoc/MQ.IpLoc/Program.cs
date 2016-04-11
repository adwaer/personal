using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using MQ.Domain;

namespace MQ.IpLoc
{
    internal class Program
    {
        private const string FilePath = "geobase.dat";
        private static void Main(string[] args)
        {
            var watch = new Stopwatch();
            watch.Start();

            Read();


            //using (var stream = File.OpenRead("geobase.dat"))
            //{
            //    WriteLog(ref start, "read");
            //DbReadMethods.ReadByMethods(ref start); // 400 ms
            //DbReadMethods.ReadByQuery(ref start); // 500 ms
            //}


            //DbReadMethods.ReadAsync(); // 400 ms
            //DbReadMethods.ReadUnsafeToJson(); // ??? ms

            WriteLog(watch.Elapsed.TotalMilliseconds, "spent");
            watch.Stop();
            Console.ReadLine();
        }

        private static int Read()
        {

            FileReader fr = new FileReader();
            if (fr.Open(FilePath))
            {
                var fileInfo = new FileInfo(FilePath);
                byte[] buffer = new byte[fileInfo.Length];
                fr.Read(buffer, 0, (int)fileInfo.Length);

                var ansiEncoding = Encoding.Default;
                int position = 0;

                var version = BitConverter.ToInt32(buffer, position);
                position += 4;

                string name = ansiEncoding.GetString(buffer, position, 32);
                position += 32;

                var makeTime = EnvironmentConstant.UnixDateTime.AddSeconds(BitConverter.ToInt64(buffer, position));
                position += 8;

                var recordCount = BitConverter.ToInt32(buffer, position);
                position += 4;

                var rangeOffset = BitConverter.ToUInt32(buffer, position);
                position += 4;

                var cityOffset = BitConverter.ToUInt32(buffer, position);
                position += 4;

                var locationOffset = BitConverter.ToUInt32(buffer, position);
                position += 4;

                var ipLocations = new IpLocation[recordCount];
                ThreadPool.QueueUserWorkItem(GetIpLocations, new IpLocParams
                {
                    Buffer = buffer,
                    Position = position,
                    RecordCount = recordCount,
                    Result = ipLocations
                });
            }

            return 0;

            byte[] buffer4 = new byte[4];
            byte[] buffer8 = new byte[8];
            byte[] buffer12 = new byte[12];
            byte[] buffer24 = new byte[24];
            byte[] buffer32 = new byte[32];

            if (fr.Open(FilePath))
            {
                // Подразумевается, что происходит чтение файла в кодировке ASCII
                //ASCIIEncoding Encoding = new ASCIIEncoding();
                var ansiEncoding = Encoding.Default;

                fr.Read(buffer4, 0, 4);
                var version = BitConverter.ToInt32(buffer4, 0);
                //position += 4;

                fr.Read(buffer32, 0, 32);
                string name = ansiEncoding.GetString(buffer32, 0, 32);

                fr.Read(buffer8, 0, 8);
                var makeTime = EnvironmentConstant.UnixDateTime.AddSeconds(BitConverter.ToInt64(buffer8, 0));

                fr.Read(buffer4, 0, 4);
                var recordCount = BitConverter.ToInt32(buffer4, 0);

                fr.Read(buffer4, 0, 4);
                var rangeOffset = BitConverter.ToUInt32(buffer4, 0);

                fr.Read(buffer4, 0, 4);
                var cityOffset = BitConverter.ToUInt32(buffer4, 0);

                fr.Read(buffer4, 0, 4);
                var locationOffset = BitConverter.ToUInt32(buffer4, 0);

                //var ipLocations = new IpLocation[recordCount];
                for (int i = 0; i < recordCount; i++)
                {
                    fr.Read(buffer8, 0, 8);
                    var fromIp = BitConverter.ToUInt64(buffer8, 0);
                    fr.Read(buffer8, 0, 8);
                    var toIp = BitConverter.ToUInt64(buffer8, 0);
                    fr.Read(buffer4, 0, 4);
                    var index = BitConverter.ToUInt32(buffer4, 0);

                    //ipLocations[i] = new IpLocation
                    //{
                    //    FromIp = fromIp,
                    //    ToIp = toIp,
                    //    Index = index
                    //};
                }

                //Location[] locations = new Location[recordCount];
                //for (int i = 0; i < recordCount; i++)
                //{
                //    fr.Read(buffer8, 0, 8);
                //    string country = ansiEncoding.GetString(buffer8, 0, 8);

                //    fr.Read(buffer12, 0, 12);
                //    string region = ansiEncoding.GetString(buffer12, 0, 12);

                //    fr.Read(buffer12, 0, 12);
                //    string postal = ansiEncoding.GetString(buffer12, 0, 12);

                //    fr.Read(buffer24, 0, 24);
                //    string city = ansiEncoding.GetString(buffer24, 0, 24);

                //    fr.Read(buffer32, 0, 32);
                //    string company = ansiEncoding.GetString(buffer32, 0, 32);

                //    fr.Read(buffer4, 0, 4);
                //    var lat = BitConverter.ToInt32(buffer4, 0);

                //    fr.Read(buffer4, 0, 4);
                //    var lon = BitConverter.ToInt32(buffer4, 0);

                //    //locations[i] = new Location
                //    //{
                //    //    Country = country,
                //    //    Region = region,
                //    //    Postal = postal,
                //    //    City = city,
                //    //    Company = company,
                //    //    Lat = lat,
                //    //    Lon = lon
                //    //};
                //}

                //int bytesRead;
                //do
                //{
                //    bytesRead = fr.Read(buffer, 0, buffer.Length);
                //    string content = Encoding.GetString(buffer, 0, bytesRead);
                //    //Console.Write("{0}", content);
                //} while (bytesRead > 0);

                fr.Close();
                return 0;
            }
            else
            {
                Console.WriteLine("Не получилось открыть запрашиваемый файл");
                return 1;
            }
        }

        private static void GetIpLocations(object data)
        {
            int recordCount, byte[] buffer, int position

            var ipLocations = new IpLocation[recordCount];
            for (int i = 0; i < recordCount; i++)
            {
                var fromIp = BitConverter.ToUInt64(buffer, position);
                position += 8;
                var toIp = BitConverter.ToUInt64(buffer, position);
                position += 8;
                var index = BitConverter.ToUInt32(buffer, position);
                position += 4;

                ipLocations[i] = new IpLocation
                {
                    FromIp = fromIp,
                    ToIp = toIp,
                    Index = index
                };
            }
            return ipLocations;
        }

        static void WriteLog(double spent, string action)
        {
            Console.WriteLine($"{action} time: {spent}");
        }

    }

    class IpLocParams
    {
        public int RecordCount;
        public int Position;
        public byte[] Buffer;
        public IpLocation[] Result;
    }

    class FileReader
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

        public bool Open(string FileName)
        {
            // open the existing file for reading          
            handle = CreateFile(FileName,
                                GENERIC_READ,
                                0,
                                0,
                                OPEN_EXISTING,
                                0,
                                0);

            if (handle != IntPtr.Zero)
                return true;
            else
                return false;
        }

        public unsafe int Read(byte[] buffer, int index, int count)
        {
            int n = 0;
            fixed (byte* p = buffer)
            {
                if (!ReadFile(handle, p + index, count, &n, 0))
                    return 0;
            }
            return n;
        }

        public bool Close()
        {
            // закрыть хендл файла
            return CloseHandle(handle);
        }
    }
}
